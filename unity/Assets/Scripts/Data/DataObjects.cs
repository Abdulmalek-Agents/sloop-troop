using UnityEngine;
using SloopTroop.Gameplay;

namespace SloopTroop.Data
{
    /// <summary>A salvage type the crew can haul and build with (GDD §5.2, §5.4).</summary>
    [CreateAssetMenu(menuName = "SloopTroop/Salvage Data", fileName = "SalvageData")]
    public class SalvageData : ScriptableObject
    {
        public string Id;
        public string DisplayName = "Crate";
        public WeightClass Weight = WeightClass.Light;
        [Min(1)] public int SalvageValue = 1;
        public Mesh Mesh;
    }

    public enum OutpostPerk { None, RevealNextIsland, BiggerNet, SecondDinghy, FasterPump }

    /// <summary>A reconstruction blueprint for an island outpost (GDD §5.4).</summary>
    [CreateAssetMenu(menuName = "SloopTroop/Outpost Blueprint", fileName = "OutpostBlueprint")]
    public class OutpostBlueprint : ScriptableObject
    {
        public string Id;
        public string DisplayName = "Lighthouse";
        [Min(1)] public int RequiredPieces = 12;
        public OutpostPerk CompletionPerk = OutpostPerk.RevealNextIsland;
        public bool RevealsNextIsland = true;
    }
}
