using UnityEngine;
using Unity.Netcode;

namespace SloopTroop.Core
{
    /// <summary>
    /// Lightweight scene bootstrap: guarantees a NetworkManager exists and survives scene loads, and is the
    /// single place to wire global references for the slice. Real session flow (Steam lobby/relay) plugs in
    /// alongside NetworkBootstrap; this just keeps the prototype launchable from any scene.
    /// </summary>
    [DefaultExecutionOrder(-500)]
    public class GameBootstrap : MonoBehaviour
    {
        public static GameBootstrap Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (NetworkManager.Singleton == null)
                Debug.LogWarning("[Bootstrap] No NetworkManager in scene — add one to Boot.unity (see ProjectStructure.md).");
        }
    }
}
