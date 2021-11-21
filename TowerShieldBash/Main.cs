using HarmonyLib;
using UnityModManagerNet;

namespace TowerShieldBash
{
    internal static class Main
    {
        private static UnityModManager.ModEntry _modEntry;

        public static bool Enabled;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            _modEntry = modEntry;
            modEntry.OnToggle = OnToggle;
            Enabled = modEntry.Enabled;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        public static void Log(string msg)
        {
            _modEntry.Logger.Log(msg);
        }
    }
}
