using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights
{
    internal class DynamicLights
    {
        public static void ApplyPatch(Harmony harmony)
        {
            var type = AccessTools.TypeByName("Dynamic_Lights.IslandStreetlightFire");
            var setLight = AccessTools.Method(type, "SetLight");

            harmony.Patch(setLight, postfix: new HarmonyMethod(typeof(DynamicLights), nameof(SetLightPostfix)));
        }

        private static void SetLightPostfix(object __instance, bool newState)
        {
            var component = (Component)__instance;
            var flicker = component.GetComponent<LanternFlicker>();
            if (flicker == null)
                return;

            flicker.SetFlicker(newState);
        }
    }
}
