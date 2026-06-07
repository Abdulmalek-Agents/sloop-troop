using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SloopTroop.Gameplay
{
    /// <summary>
    /// Buoyancy + trim for the shared sloop (GDD §5.3). Samples a set of float points against a water plane to
    /// keep the deck afloat, then biases the righting force by **trim** (how lopsided the current cargo is).
    /// Overload one side and the deck lists, sliding loose cargo overboard. Capsize is a soft-fail. Runs on the
    /// server only (authoritative physics); clients see the synced transform.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SloopBuoyancy : NetworkBehaviour
    {
        [SerializeField] private Transform[] _floatPoints;
        [SerializeField] private float _waterLevel = 0f;
        [SerializeField] private float _buoyancyForce = 14f;
        [SerializeField] private float _waterDrag = 1.2f;
        [SerializeField] private float _capsizeAngle = 55f;

        private Rigidbody _rb;

        /// <summary>0 = even keel, ±1 = fully listed. Driven by cargo distribution; read by HUD/SwellDirector.</summary>
        public float Trim { get; private set; }

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            if (!IsServer || _floatPoints == null) return;

            foreach (var p in _floatPoints)
            {
                float depth = _waterLevel - p.position.y;
                if (depth <= 0f) continue;
                float lift = _buoyancyForce * Mathf.Clamp01(depth);
                _rb.AddForceAtPosition(Vector3.up * lift, p.position, ForceMode.Acceleration);
                var v = _rb.GetPointVelocity(p.position);
                _rb.AddForceAtPosition(-v * _waterDrag, p.position, ForceMode.Acceleration);
            }

            // current list angle around the forward axis -> normalized Trim
            float roll = Vector3.SignedAngle(transform.up, Vector3.up, transform.forward);
            Trim = Mathf.Clamp(roll / _capsizeAngle, -1f, 1f);
        }

        /// <summary>Called by the Swell director to shove the hull (and everything loose on it).</summary>
        public void ServerApplySwell(Vector3 impulse)
        {
            if (!IsServer) return;
            _rb.AddForce(impulse, ForceMode.VelocityChange);
            _rb.AddTorque(transform.forward * impulse.magnitude * 0.4f, ForceMode.VelocityChange);
        }

        public bool Capsized => Mathf.Abs(Trim) >= 0.98f;
    }
}
