using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights
{
    internal class StreetLightPatches
    {
        [HarmonyPatch(typeof(IslandStreetlight))]
        internal class IslandStreetlightPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Awake")]
            public static void AddFlicker(IslandStreetlight __instance, Light ___light)
            {
                if (___light.name == "senna_lantern_light")
                    return;

                __instance.gameObject.AddComponent<LanternFlicker>();
            }

            [HarmonyPostfix]
            [HarmonyPatch("SetLight")]
            public static void AddFlickerToLight(IslandStreetlight __instance, bool newState)
            {
                var flicker = __instance.GetComponent<LanternFlicker>();
                if (flicker == null)
                    return;

                flicker.SetFlicker(newState);
            }
        }
    }
}
