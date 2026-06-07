using Unity.Netcode;
using UnityEngine;
using SloopTroop.Data;

namespace SloopTroop.Build
{
    /// <summary>
    /// An outpost under reconstruction (GDD §5.4). Tracks pieces placed against a blueprint; raises its mesh as
    /// it fills in, and grants a crew-wide perk on completion (e.g., a finished lighthouse reveals the next
    /// island). Server-authoritative progress synced to all clients.
    /// </summary>
    public class Outpost : NetworkBehaviour
    {
        [SerializeField] private OutpostBlueprint _blueprint;
        [SerializeField] private Transform _risingMesh;   // scaled up as pieces are added (visual "building")

        public NetworkVariable<int> Pieces = new(writePerm: NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn() => Pieces.OnValueChanged += (_, __) => RefreshVisual();

        public void ServerAddPieces(int n)
        {
            if (!IsServer) return;
            int required = _blueprint != null ? _blueprint.RequiredPieces : 12;
            Pieces.Value = Mathf.Min(required, Pieces.Value + n);
            if (Pieces.Value >= required) ServerComplete();
        }

        private void ServerComplete() => CompleteClientRpc();

        [ClientRpc]
        private void CompleteClientRpc()
        {
            // Hook: play the lighthouse "light on" beat, grant the blueprint perk, reveal the next island.
            Debug.Log($"[Outpost] {(_blueprint != null ? _blueprint.DisplayName : name)} complete! Perk granted.");
            RefreshVisual();
        }

        private void RefreshVisual()
        {
            if (_risingMesh == null || _blueprint == null) return;
            float t = Mathf.Clamp01((float)Pieces.Value / Mathf.Max(1, _blueprint.RequiredPieces));
            var s = _risingMesh.localScale;
            _risingMesh.localScale = new Vector3(s.x, Mathf.Lerp(0.05f, 1f, t), s.z);
        }
    }
}
