using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights
{
    internal class InteriorPatches
    {
        [HarmonyPatch(typeof(InteriorEffectsTrigger))]
        internal static class InteriorEffectsTriggerPatch
        {
            [HarmonyPatch("OnTriggerEnter")]
            [HarmonyPostfix]
            private static void OnTriggerEnter(Collider other)
            {
                if (other == null)
                    return;

                var flicker = other.GetComponentInParent<LanternFlicker>();
                if (flicker == null)
                    return;

                flicker.isInside = true;
            }

            [HarmonyPatch("OnTriggerExit")]
            [HarmonyPostfix]
            private static void OnTriggerExit(Collider other)
            {
                if (other == null)
                    return;

                var flicker = other.GetComponentInParent<LanternFlicker>();
                if (flicker == null)
                    return;

                flicker.isInside = false;
            }
        }
    }
}
