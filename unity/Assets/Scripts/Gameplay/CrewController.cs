using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using SloopTroop.Networking;

namespace SloopTroop.Gameplay
{
    /// <summary>
    /// A networked crew member (GDD §5.2). Server-authoritative movement: the owner samples input and sends it
    /// to the server, which moves the body and reconciles. Handles Grab (carry / co-carry) and Yell (widen
    /// proximity voice + summon a co-carrier). Mirrors the HTML prototype's verbs in 3D.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class CrewController : NetworkBehaviour
    {
        [SerializeField] private float _moveForce = 28f;
        [SerializeField] private float _maxSpeed = 4.5f;
        [SerializeField] private Transform _hand;
        [SerializeField] private ProximityVoice _voice;

        private Rigidbody _rb;
        private Vector2 _moveInput;
        private Carryable _held;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        public override void OnNetworkSpawn()
        {
            // Only the server simulates physics bodies; remote/owner copies are kinematic followers.
            _rb.isKinematic = !IsServer;
            if (!IsOwner) enabled = enabled && true; // keep enabled so we still draw; input gated below
        }

        // ---- Input System (owner only) -------------------------------------------------------
        public void OnMove(InputValue v) { if (IsOwner) _moveInput = v.Get<Vector2>(); }
        public void OnGrab(InputValue v) { if (IsOwner && v.isPressed) GrabServerRpc(); }
        public void OnYell(InputValue v) { if (IsOwner) YellServerRpc(v.isPressed); }

        private void FixedUpdate()
        {
            if (!IsOwner) return;
            // Owner forwards intent; server applies it (cheap, forgiving — capsize is a soft-fail by design).
            MoveServerRpc(_moveInput);
        }

        [ServerRpc]
        private void MoveServerRpc(Vector2 input)
        {
            var dir = new Vector3(input.x, 0f, input.y);
            // heavy beam carried solo halves speed until a second crew member co-carries (GDD §5.2)
            float speedMul = (_held != null && _held.IsHeavy && _held.CarrierCount < 2) ? 0.5f : 1f;
            _rb.AddForce(dir * (_moveForce * speedMul), ForceMode.Acceleration);
            var flat = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            if (flat.magnitude > _maxSpeed * speedMul)
            {
                flat = flat.normalized * _maxSpeed * speedMul;
                _rb.linearVelocity = new Vector3(flat.x, _rb.linearVelocity.y, flat.z);
            }
        }

        [ServerRpc]
        private void GrabServerRpc()
        {
            if (_held != null) { _held.ServerRelease(this); _held = null; return; }
            var found = Carryable.FindNearest(_hand != null ? _hand.position : transform.position, 1.4f);
            if (found != null && found.ServerTryGrab(this, _hand != null ? _hand : transform))
                _held = found;
        }

        [ServerRpc]
        private void YellServerRpc(bool pressed)
        {
            if (_voice != null) { _voice.SetYell(pressed); _voice.IsSpeaking.Value = pressed; }
            // A yell while holding a heavy beam flags it as needing a co-carrier (AI/crew can answer).
            if (pressed && _held != null && _held.IsHeavy) _held.ServerRequestHelp();
        }

        public Carryable Held => _held;
    }
}
