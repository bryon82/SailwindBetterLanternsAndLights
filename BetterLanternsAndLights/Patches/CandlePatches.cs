using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights.Patches
{
    internal class CandlePatches
    {
        [HarmonyPatch(typeof(IslandStreetlightsManager))]
        internal class IslandStreetlightPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Awake")]
            public static void AddFlicker(IslandStreetlightsManager __instance)
            {
                var lights = __instance.GetComponentsInChildren<Light>();
                foreach (var light in lights)
                {
                    if (light.transform.parent.name.Contains("candle"))
                    {
                        var flicker = light.gameObject.AddComponent<LanternFlicker>();
                        flicker.SetFlicker(true);
                    }
                }
            }
        }
    }
}
