using HarmonyLib;
using UnityEngine;

namespace BetterLanternsAndLights.Patches
{
    internal class StovePatches
    {
        [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
        internal class AddPaperLanternFlickerPatch
        {
            public static void Postfix(PrefabsDirectory __instance)
            {
                var stoves = new int[] { 105, 106, 107, 377, 378, 379 };

                foreach (var stoveId in stoves)
                    __instance.directory[stoveId].AddComponent<LanternFlicker>();
            }
        }

        [HarmonyPatch(typeof(ShipItemStove))]
        internal class StoveFuelTriggerUpdatePatch
        {
            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void SetLightFlicker(ShipItemStove __instance, StoveFuelTrigger ___fuelTrigger)
            {
                if (___fuelTrigger == null)
                    return;

                var flicker = __instance.GetComponent<LanternFlicker>();
                if (flicker == null || flicker.flickerCoroutine == null)
                    return;

                ___fuelTrigger.GetPrivateField<Light>("light").intensity = flicker.lightIntensity;
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnItemClick")]
            public static void ToggleFlicker(ShipItemStove __instance, StoveFuelTrigger ___fuelTrigger, PickupableItem heldItem)
            {
                if (___fuelTrigger == null)
                    return;

                var flicker = __instance.GetComponent<LanternFlicker>();
                if (flicker == null || flicker.flickerCoroutine != null)
                    return;

                if (heldItem.GetComponent<StoveFuel>() == null)
                    return;

                flicker.maxIntensity = 
                    ___fuelTrigger.GetPrivateField<int>("currentFuel")
                    / ___fuelTrigger.GetPrivateField<int>("maxFuel")
                    * ___fuelTrigger.GetPrivateField<float>("initialLightIntensity");

                flicker.SetFlicker(true);
            }
        }

        [HarmonyPatch(typeof(StoveFuelTrigger), "UnregisterBurntFuel")]
        internal class StoveFuelTriggerUnregisterBurntFuelPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch("UnregisterBurntFuel")]
            public static void StopFlicker(ShipItemStove ___stove, float ___initialLightIntensity, int ___currentFuel, int ___maxFuel)
            {
                var flicker = ___stove.GetComponent<LanternFlicker>();
                if (flicker == null)
                    return;

                flicker.maxIntensity = ___currentFuel / ___maxFuel * ___initialLightIntensity;

                if (___currentFuel <= 0)
                    flicker.SetFlicker(false);
            }
        }
    }
}
