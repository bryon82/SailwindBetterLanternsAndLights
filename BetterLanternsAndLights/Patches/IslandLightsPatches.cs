using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights
{
    internal class IslandLightsPatches
    {
        [HarmonyPatch(typeof(IslandStreetlightsManager), "Awake")]
        internal class IslandlightsAwakePatch
        {
            public static void Postfix(IslandStreetlightsManager __instance)
            {
                var lights = __instance.GetComponentsInChildren<Light>();
                foreach (var light in lights)
                {
                    var lightGetsFicker =
                        light.transform.parent.name.Contains("candle")
                        || light.name.Contains("Brazier")
                        || light.transform.parent.name.Contains("shop stove");

                    if (lightGetsFicker)
                    {
                        var flicker = light.gameObject.AddComponent<LanternFlicker>();
                        flicker.SetFlicker(true);
                    }
                }
            }
        }
    }
}
