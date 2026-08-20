using HarmonyLib;
using UnityEngine;
using static BetterLanternsAndLights.BLL_Plugin;

namespace BetterLanternsAndLights
{
    internal class PlayerLightPatches
    {
        [HarmonyPatch(typeof(PlayerControllerMirror), "Awake")]
        internal class PlayerControllerMirrorAwakePatch
        {
            public static void Postfix()
            {
                var camera = Camera.main;
                PlayerLight = camera?.GetComponent<Light>();
            }
        }
    }
}
