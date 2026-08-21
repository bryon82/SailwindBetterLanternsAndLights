using HarmonyLib;

namespace BetterLanternsAndLights
{
    internal class LanternPatches
    {
        [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
        internal class AddPaperLanternFlickerPatch
        {            
            public static void Postfix(PrefabsDirectory __instance)
            {
                var lanterns = new int[] { 110, 111, 112, 113, 114, 133, 134 };

                foreach (var lanternId in lanterns)
                    __instance.directory[lanternId].AddComponent<LanternFlicker>();
            }
        }

        [HarmonyPatch(typeof(ShipItemLight), "SetLight")]
        internal class ShipItemLightSetLightPatch
        {
            public static void Postfix(ShipItemLight __instance, bool state)
            {
                var flicker = __instance.GetComponent<LanternFlicker>();
                if (flicker == null)
                    return;

                flicker.SetFlicker(state);
            }
        }

        [HarmonyPatch(typeof(GPButtonInventorySlot), "Awake")]
        public class AddInventoryLantern
        {
            public static void Postfix(GPButtonInventorySlot __instance)
            {
                var inventoryLantern = __instance.gameObject.AddComponent<InventoryLantern>();
                inventoryLantern.InventorySlot = __instance;
            }
        }
    }
}
