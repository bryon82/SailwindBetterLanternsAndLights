using BepInEx.Configuration;
using UnityEngine.SceneManagement;
using static BetterLanternsAndLights.BLL_Plugin;

namespace BetterLanternsAndLights
{
    internal class Configs
    {
        internal static ConfigEntry<bool> enableLanternFlicker;
        internal static ConfigEntry<bool> addDcGateLights;
        internal static ConfigEntry<bool> enablePlayerLight;
        internal static ConfigEntry<bool> enableInventoryLantern;

        internal static void InitializeConfigs()
        {
            var config = Instance.Config;

            enableLanternFlicker = config.Bind(
                "Settings",
                "Enable lantern flicker",
                true);

            addDcGateLights = config.Bind(
                "Settings",
                "Add Dragon Cliffs Gate Lights",
                true);

            enablePlayerLight = config.Bind(
                "Settings",
                "Ambient Player Light",
                false,
                "Light emanating from the player.");

            enableInventoryLantern = config.Bind(
                "Settings",
                "Activate Lantern In Inventory",
                true,
                "The ability to activate the lantern while it is in an inventory slot.");

            enablePlayerLight.SettingChanged += (sender, args) => UpdatePlayerLight();
            addDcGateLights.SettingChanged += (sender, args) => UpdateDCGateLights();            
        }

        internal static void UpdateDCGateLights()
        {
            if (SceneManager.GetSceneByName(DragonCliffs.DRAGON_CLIFFS_SCENE).isLoaded)
                foreach (var light in DragonCliffs.DCGateLightGOs)
                    if (light != null) light.SetActive(addDcGateLights.Value);            
        }

        internal static void UpdatePlayerLight()
        {
            if (PlayerLight != null)
                PlayerLight.enabled = enablePlayerLight.Value;
        }
    }
}
