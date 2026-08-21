using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BetterLanternsAndLights.BLL_Plugin;

namespace BetterLanternsAndLights
{
    internal class DragonCliffs
    {
        internal static List<GameObject> DCGateLightGOs { get; private set; }
        internal const string DRAGON_CLIFFS_SCENE = "island 9 E Dragon Cliffs";

        internal static void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            if (scene.name == DRAGON_CLIFFS_SCENE)
            {
                Initialize();
            }
        }

        internal static void Initialize()
        {
            DCGateLightGOs = new List<GameObject>();
            var positions = new Vector3[2]
            {
                new Vector3(0f, 3.35f, 3.1f),
                new Vector3(0f, -3.35f, 3.1f)
            };

            var scenery = GameObject.Find("island 9 E (dragon cliffs) scenery");
            var parent = scenery.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals("east_gate (4)"));
            var light = scenery.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals("east_street_rope (1)")).GetChild(0);
            foreach (var position in positions)
            {
                var gateLight = Object.Instantiate(light.gameObject, parent);
                gateLight.transform.localScale = new Vector3(1.33f, 1.33f, 1.33f);
                gateLight.transform.localPosition = position;
                gateLight.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                gateLight.GetComponent<Light>().range = 50f;
                gateLight.GetComponent<Light>().intensity = 1.5f;
                DCGateLightGOs.Add(gateLight);
            }
            LogDebug("Dragon Cliffs gate lights added");
        }
    }
}
