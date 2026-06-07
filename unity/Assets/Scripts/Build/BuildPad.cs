using Unity.Netcode;
using UnityEngine;
using SloopTroop.Core;
using SloopTroop.Gameplay;

namespace SloopTroop.Build
{
    /// <summary>
    /// The Build Pad (GDD §5.4). A trigger volume on the island: when a crew member brings a Carryable into it,
    /// the server consumes the salvage and advances the Outpost. Collaborative by nature — six people funnelling
    /// awkward cargo into one spot is the point.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BuildPad : NetworkBehaviour
    {
        [SerializeField] private Outpost _outpost;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;
            var salvage = other.GetComponentInParent<Carryable>();
            if (salvage == null) return;
            DeliverServerRpc(new NetworkObjectReference(salvage.NetworkObject));
        }

        [ServerRpc(RequireOwnership = false)]
        private void DeliverServerRpc(NetworkObjectReference salvageRef)
        {
            if (!salvageRef.TryGet(out var netObj)) return;
            var salvage = netObj.GetComponent<Carryable>();
            if (salvage == null) return;

            salvage.ServerReleaseAll();
            int pieces = salvage.SalvageValue;

            if (_outpost != null) _outpost.ServerAddPieces(pieces);
            if (MatchManager.Instance != null) MatchManager.Instance.ServerAddBuildPiece(pieces);

            netObj.Despawn();   // salvage becomes part of the structure
        }
    }
}
