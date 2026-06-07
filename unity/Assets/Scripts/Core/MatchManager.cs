using System;
using Unity.Netcode;
using UnityEngine;

namespace SloopTroop.Core
{
    /// <summary>
    /// Authoritative, networked run state for a Sloop Troop expedition (GDD §4, §5.6). Holds the shared
    /// build progress, current expedition number, and goal in NetworkVariables so every client's HUD agrees.
    /// The server is the only writer; clients read and react.
    /// </summary>
    public class MatchManager : NetworkBehaviour
    {
        public static MatchManager Instance { get; private set; }

        [SerializeField] private int _baseGoal = 12;
        [SerializeField] private int _goalPerExpedition = 4;

        public NetworkVariable<int> BuildProgress = new(writePerm: NetworkVariableWritePermission.Server);
        public NetworkVariable<int> Goal = new(12, writePerm: NetworkVariableWritePermission.Server);
        public NetworkVariable<int> Expedition = new(1, writePerm: NetworkVariableWritePermission.Server);

        /// <summary>Client-side event so HUD/audio can celebrate when a piece lands or an island completes.</summary>
        public event Action<int, int> OnProgressChanged;   // (progress, goal)
        public event Action<int> OnIslandComplete;          // (new expedition)

        private void Awake() => Instance = this;

        public override void OnNetworkSpawn()
        {
            BuildProgress.OnValueChanged += (_, v) => OnProgressChanged?.Invoke(v, Goal.Value);
            if (IsServer)
            {
                Expedition.Value = 1;
                Goal.Value = _baseGoal + _goalPerExpedition;
                BuildProgress.Value = 0;
            }
        }

        /// <summary>Server-only: a salvage piece was delivered to the Build Pad.</summary>
        public void ServerAddBuildPiece(int amount = 1)
        {
            if (!IsServer) return;
            BuildProgress.Value += amount;
            if (BuildProgress.Value >= Goal.Value)
                ServerCompleteIsland();
        }

        private void ServerCompleteIsland()
        {
            Expedition.Value++;
            Goal.Value = _baseGoal + _goalPerExpedition * Expedition.Value;
            BuildProgress.Value = 0;
            IslandCompleteClientRpc(Expedition.Value);
        }

        [ClientRpc]
        private void IslandCompleteClientRpc(int newExpedition) => OnIslandComplete?.Invoke(newExpedition);
    }
}
