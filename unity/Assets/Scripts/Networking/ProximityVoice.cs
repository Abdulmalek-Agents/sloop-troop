using Unity.Netcode;
using UnityEngine;

namespace SloopTroop.Networking
{
    /// <summary>
    /// Proximity voice hook (GDD §5.1 — voice is THE core mechanic, not a feature). This is a transport-agnostic
    /// stub: plug in Vivox or Dissonance under <see cref="ApplyAttenuation"/>. It computes distance-based volume
    /// (with a "yell" radius boost) and exposes an <see cref="IsSpeaking"/> flag the avatar uses to draw the
    /// speaking ring. Keeping it isolated means the comedy-critical voice layer can be swapped without touching
    /// gameplay code.
    /// </summary>
    public class ProximityVoice : NetworkBehaviour
    {
        [SerializeField] private float _hearRadius = 14f;
        [SerializeField] private float _yellMultiplier = 2.2f;
        [SerializeField] private float _minVolume = 0f;

        /// <summary>True while this crew member is transmitting (drives the on-avatar speaking ring).</summary>
        public NetworkVariable<bool> IsSpeaking = new(writePerm: NetworkVariableWritePermission.Owner);

        private float _radiusBoost = 1f;

        /// <summary>Called by CrewController on "yell" to temporarily widen the voice radius.</summary>
        public void SetYell(bool yelling) => _radiusBoost = yelling ? _yellMultiplier : 1f;

        /// <summary>
        /// Returns 0..1 volume for THIS speaker as heard by <paramref name="listener"/>. The chosen voice SDK
        /// applies it per-channel. Linear falloff keeps "shout across the deck" readable and funny.
        /// </summary>
        public float VolumeFor(Transform listener)
        {
            float r = _hearRadius * _radiusBoost;
            float d = Vector3.Distance(transform.position, listener.position);
            return Mathf.Clamp(1f - d / r, _minVolume, 1f);
        }

        public void ApplyAttenuation(Transform listener, AudioSource sdkChannel)
        {
            if (sdkChannel != null) sdkChannel.volume = VolumeFor(listener);
        }
    }
}
