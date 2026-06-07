using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SloopTroop.Gameplay
{
    public enum WeightClass { Light, Heavy }

    /// <summary>
    /// A networked salvage object (GDD §5.2). Server-authoritative rigidbody; grabbing creates a forgiving
    /// joint to a carrier's hand. **Heavy** items support co-carry (≥2 carriers) — when carried by an
    /// uncoordinated pair they swing wildly, which is the comedy. Delivered to a BuildPad to become outpost
    /// progress.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Carryable : NetworkBehaviour
    {
        private static readonly List<Carryable> All = new();

        [SerializeField] private WeightClass _weight = WeightClass.Light;
        [SerializeField] private int _salvageValue = 1;
        [SerializeField] private float _jointBreakForce = Mathf.Infinity;

        private Rigidbody _rb;
        private readonly List<(CrewController crew, ConfigurableJoint joint)> _carriers = new();

        public NetworkVariable<bool> NeedsHelp = new();

        public bool IsHeavy => _weight == WeightClass.Heavy;
        public int CarrierCount => _carriers.Count;
        public int SalvageValue => _salvageValue;

        private void Awake() => _rb = GetComponent<Rigidbody>();
        private void OnEnable() => All.Add(this);
        private void OnDisable() => All.Remove(this);

        public static Carryable FindNearest(Vector3 p, float maxDist)
        {
            Carryable best = null; float bd = maxDist;
            foreach (var c in All)
            {
                if (c.IsHeld && !c.IsHeavy) continue;          // single-grab for light items
                float d = Vector3.Distance(c.transform.position, p);
                if (d < bd) { bd = d; best = c; }
            }
            return best;
        }

        public bool IsHeld => _carriers.Count > 0;

        /// <summary>Server: attach a carrier. Light items allow one carrier; heavy items allow up to two.</summary>
        public bool ServerTryGrab(CrewController crew, Transform hand)
        {
            if (!IsServer) return false;
            int max = IsHeavy ? 2 : 1;
            if (_carriers.Count >= max) return false;

            var joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = hand.GetComponentInParent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.zero;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Limited;
            joint.breakForce = _jointBreakForce;
            _carriers.Add((crew, joint));
            if (_carriers.Count >= 2) NeedsHelp.Value = false;
            return true;
        }

        public void ServerRelease(CrewController crew)
        {
            if (!IsServer) return;
            for (int i = _carriers.Count - 1; i >= 0; i--)
                if (_carriers[i].crew == crew)
                {
                    if (_carriers[i].joint != null) Destroy(_carriers[i].joint);
                    _carriers.RemoveAt(i);
                }
        }

        public void ServerReleaseAll()
        {
            if (!IsServer) return;
            foreach (var c in _carriers) if (c.joint != null) Destroy(c.joint);
            _carriers.Clear();
        }

        /// <summary>Flag for AI/crew that a heavy beam wants a second pair of hands (driven by a "yell").</summary>
        public void ServerRequestHelp() { if (IsServer && IsHeavy && _carriers.Count < 2) NeedsHelp.Value = true; }
    }
}
