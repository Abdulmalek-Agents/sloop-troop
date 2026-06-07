using UnityEngine;
using UnityEngine.UIElements;
using SloopTroop.Core;
using SloopTroop.Gameplay;
using SloopTroop.Director;

namespace SloopTroop.UI
{
    /// <summary>
    /// Binds networked run state to a UI Toolkit HUD: build progress, deck trim, swell warning, expedition and
    /// crew count. Mirrors the HTML prototype's HUD so the networked build feels identical to the proto.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private SloopBuoyancy _sloop;
        [SerializeField] private SwellDirector _swell;

        private Label _build, _trim, _exp, _crew;
        private VisualElement _buildFill, _trimFill, _swellBanner;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _build = root.Q<Label>("build");
            _trim  = root.Q<Label>("trim");
            _exp   = root.Q<Label>("exp");
            _crew  = root.Q<Label>("crew");
            _buildFill   = root.Q<VisualElement>("build-fill");
            _trimFill    = root.Q<VisualElement>("trim-fill");
            _swellBanner = root.Q<VisualElement>("swell-banner");

            if (_swell != null)
            {
                _swell.OnSwellWarned += () => ShowBanner(true);
                _swell.OnSwellHit    += () => ShowBanner(false);
            }
        }

        private void ShowBanner(bool show)
        {
            if (_swellBanner != null) _swellBanner.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void Update()
        {
            var mm = MatchManager.Instance;
            if (mm != null)
            {
                int p = mm.BuildProgress.Value, g = Mathf.Max(1, mm.Goal.Value);
                if (_build != null) _build.text = p + " / " + g;
                if (_buildFill != null) _buildFill.style.width = Length.Percent(Mathf.Clamp01((float)p / g) * 100f);
                if (_exp != null) _exp.text = mm.Expedition.Value.ToString();
            }

            if (_sloop != null)
            {
                float t = Mathf.Abs(_sloop.Trim);
                if (_trimFill != null) _trimFill.style.width = Length.Percent(t * 100f);
                if (_trim != null) _trim.text = t < 0.33f ? "steady" : t < 0.7f ? "listing!" : "CAPSIZING!";
            }

            if (_crew != null && Unity.Netcode.NetworkManager.Singleton != null)
                _crew.text = Unity.Netcode.NetworkManager.Singleton.ConnectedClientsList.Count.ToString();
        }
    }
}
