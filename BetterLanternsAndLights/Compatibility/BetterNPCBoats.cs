using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights
{
    internal class BetterNPCBoats
    {
        public static void ApplyPatch(Harmony harmony)
        {
            var type = AccessTools.TypeByName("BetterNPCBoats.BetterNPCFishingBoat");
            var toggleLanterns = AccessTools.Method(type, "ToggleLanterns");

            harmony.Patch(toggleLanterns, postfix: new HarmonyMethod(typeof(BetterNPCBoats), nameof(ToggleLanternsPostfix)));
        }

        private static void ToggleLanternsPostfix(object __instance, bool state)
        {
            var component = (Component)__instance;
            var flicker = component.GetComponent<LanternFlicker>();
            if (flicker == null)
                return;

            flicker.SetFlicker(state);
        }
    }
}
