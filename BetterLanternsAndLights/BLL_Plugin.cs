using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterLanternsAndLights
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(DYNAMIC_LIGHTS_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(BETTER_NPC_BOATS_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class BLL_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude.betterlanternsandlights";
        public const string PLUGIN_NAME = "Better Lanterns and Lights";
        public const string PLUGIN_VERSION = "1.0.0";

        public const string DYNAMIC_LIGHTS_GUID = "com.nandbrew.dynamiclights";
        public const string BETTER_NPC_BOATS_GUID = "com.raddude.betternpcboats";

        internal const string DRAGON_CLIFFS_SCENE = "island 9 E Dragon Cliffs";
        public static Light PlayerLight { get; internal set; }

        internal static BLL_Plugin Instance { get; private set; }
        private static ManualLogSource _logger;

        internal static void LogDebug(string message) => _logger.LogDebug(message);
        internal static void LogInfo(string message) => _logger.LogInfo(message);
        internal static void LogWarning(string message) => _logger.LogWarning(message);
        internal static void LogError(string message) => _logger.LogError(message);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _logger = Logger;

            Configs.InitializeConfigs();
            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);

            var dynamicLightsLoaded = false;
            var betterNPCBoatsLoaded = false;
            foreach (var plugin in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
            {
                if (plugin.Metadata.GUID == DYNAMIC_LIGHTS_GUID)
                {
                    LogInfo("Enabling compatibility with Dynamic Lights.");
                    DynamicLights.ApplyPatch(harmony);
                    dynamicLightsLoaded = true;
                }
                else if (plugin.Metadata.GUID == BETTER_NPC_BOATS_GUID)
                {
                    LogInfo("Enabling compatibility with Better NPC Boats.");
                    BetterNPCBoats.ApplyPatch(harmony);
                    betterNPCBoatsLoaded = true;
                }
                if (dynamicLightsLoaded && betterNPCBoatsLoaded)
                    break;
            }

            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void Update()
        {
            Configs.UpdateConfigs();
        }

        private void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            if (scene.name == DRAGON_CLIFFS_SCENE)
            {
                DragonCliffs.Initialize();
            }
        }
    }
}
