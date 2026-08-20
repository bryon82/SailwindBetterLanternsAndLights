using BepInEx.Configuration;
using UnityEngine.SceneManagement;
using static BetterLanternsAndLights.BLL_Plugin;

namespace BetterLanternsAndLights
{
    internal class Configs
    {
        internal static ConfigEntry<bool> enableLanternFlicker;
        internal static ConfigEntry<bool> addDcGateLamps;
        internal static ConfigEntry<bool> enablePlayerLight;
        internal static ConfigEntry<bool> enableInventoryLantern;

        private static bool _lastAddDcGateLamps;

        internal static void InitializeConfigs()
        {
            var config = Instance.Config;

            enableLanternFlicker = config.Bind(
                "Settings",
                "Enable lantern flicker",
                true);

            addDcGateLamps = config.Bind(
                "Settings",
                "Add Dragon Cliffs Gate Lamps",
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
                "The ability to activate the lantern in an inventory slot by right-clicking it (or using other activation button).");

            _lastAddDcGateLamps = addDcGateLamps.Value;
        }

        internal static void UpdateConfigs()
        {
            var currentAddDcGateLamps = addDcGateLamps.Value;
            if (currentAddDcGateLamps != _lastAddDcGateLamps && DragonCliffs.DCGateLampGOs != null)
            {
                if (SceneManager.GetSceneByName(DRAGON_CLIFFS_SCENE).isLoaded)
                    DragonCliffs.DCGateLampGOs.ForEach(lamp => lamp?.SetActive(currentAddDcGateLamps));

                _lastAddDcGateLamps = currentAddDcGateLamps;
            }

            if (PlayerLight != null && PlayerLight.enabled != enablePlayerLight.Value)
            {
                PlayerLight.enabled = enablePlayerLight.Value;
            }
        }
    }
}
