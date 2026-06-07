using System;
using Unity.Netcode;
using UnityEngine;
using SloopTroop.Gameplay;

namespace SloopTroop.Director
{
    /// <summary>
    /// The Swell — a lightweight, server-driven chaos director (GDD §5.5). Periodically builds a telegraphed
    /// wave that shoves the sloop and everything loose on it. Cadence tightens when the crew gets greedy (lots
    /// of loose, un-built cargo), so failure feels earned and hilarious rather than random. Always warns first.
    /// </summary>
    public class SwellDirector : NetworkBehaviour
    {
        [SerializeField] private SloopBuoyancy _sloop;
        [SerializeField] private float _minInterval = 8f;
        [SerializeField] private float _maxInterval = 12f;
        [SerializeField] private float _warnLead = 2.2f;
        [SerializeField] private float _baseImpulse = 6f;

        /// <summary>Fired on all clients when a swell is telegraphed (HUD banner, gull SFX, horizon darken).</summary>
        public event Action OnSwellWarned;
        public event Action OnSwellHit;

        private float _timer;
        private bool _warned;

        public override void OnNetworkSpawn()
        {
            if (IsServer) _timer = UnityEngine.Random.Range(_minInterval, _maxInterval);
        }

        private void Update()
        {
            if (!IsServer) return;
            _timer -= Time.deltaTime;

            if (!_warned && _timer <= _warnLead) { _warned = true; WarnClientRpc(); }
            if (_timer <= 0f) { Hit(); _warned = false; _timer = NextInterval(); }
        }

        private float NextInterval()
        {
            // greedier crew (server may pass a "loose cargo" factor here) => shorter fuse. Placeholder = base.
            return UnityEngine.Random.Range(_minInterval, _maxInterval);
        }

        private void Hit()
        {
            var dir = new Vector3(1f, 0.15f, 0f).normalized;     // sweeps to starboard
            if (_sloop != null) _sloop.ServerApplySwell(dir * _baseImpulse);
            HitClientRpc();
        }

        [ClientRpc] private void WarnClientRpc() => OnSwellWarned?.Invoke();
        [ClientRpc] private void HitClientRpc() => OnSwellHit?.Invoke();
    }
}
