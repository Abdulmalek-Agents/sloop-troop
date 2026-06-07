using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace SloopTroop.Networking
{
    /// <summary>
    /// Minimal host/join entry point for the co-op session (GDD §5.1, README §2). Wraps Netcode for
    /// GameObjects + Unity Transport. In a shipping build this is replaced by Steam lobbies / relay, but the
    /// API surface (StartHost / StartClient) stays the same.
    /// </summary>
    public class NetworkBootstrap : MonoBehaviour
    {
        [SerializeField] private string _address = "127.0.0.1";
        [SerializeField] private ushort _port = 7777;

        private UnityTransport Transport =>
            NetworkManager.Singleton.GetComponent<UnityTransport>();

        public void Host()
        {
            ConfigureTransport();
            NetworkManager.Singleton.StartHost();
            Debug.Log("[Net] Hosting on " + _address + ":" + _port);
        }

        public void Join()
        {
            ConfigureTransport();
            NetworkManager.Singleton.StartClient();
            Debug.Log("[Net] Joining " + _address + ":" + _port);
        }

        public void Leave()
        {
            if (NetworkManager.Singleton.IsListening)
                NetworkManager.Singleton.Shutdown();
        }

        private void ConfigureTransport() => Transport.SetConnectionData(_address, _port);

        // Simple dev GUI so the slice is testable without scene UI wiring.
        private void OnGUI()
        {
            if (NetworkManager.Singleton == null || NetworkManager.Singleton.IsListening) return;
            GUILayout.BeginArea(new Rect(12, 12, 180, 120));
            if (GUILayout.Button("Host")) Host();
            if (GUILayout.Button("Join")) Join();
            GUILayout.EndArea();
        }
    }
}
