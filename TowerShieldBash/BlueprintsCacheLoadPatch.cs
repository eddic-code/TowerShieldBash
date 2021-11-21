// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;

namespace TowerShieldBash
{
    [HarmonyPatch(typeof(BlueprintsCache), "Load")]
    internal static class BlueprintsCacheLoadPatch
    {
        private static BlueprintShieldType _towerShieldType;
        private static BlueprintItemWeaponReference _heavyShieldWeaponRef;

        private static BlueprintShieldType TowerShieldType => _towerShieldType ??= GetBlueprint<BlueprintShieldType>("5f0f4b6e480e7054b8592b5a8b55854a");

        private static BlueprintItemWeaponReference HeavyShieldWeaponRef => _heavyShieldWeaponRef ??= GetBlueprint<BlueprintItemWeapon>("ff8047f887565284e93773b4a698c393")
            .ToReference<BlueprintItemWeaponReference>();

        public static void Postfix(ref SimpleBlueprint __result)
        {
            if (Main.Enabled && __result is BlueprintItemShield shield && shield.Type == TowerShieldType 
                && !string.IsNullOrWhiteSpace(shield.m_DisplayNameText) && !shield.m_WeaponComponent.Equals(HeavyShieldWeaponRef))
            {
                shield.m_WeaponComponent = HeavyShieldWeaponRef;
            }
        }

        private static T GetBlueprint<T>(string guid) where T : SimpleBlueprint 
        {
            var assetGuid = new BlueprintGuid(System.Guid.Parse(guid));
            var asset = ResourcesLibrary.TryGetBlueprint(assetGuid) as T;
            if (asset == null) { Main.Log($"ERROR: Could not load asset: {guid}"); }

            return asset;
        }
    }
}
