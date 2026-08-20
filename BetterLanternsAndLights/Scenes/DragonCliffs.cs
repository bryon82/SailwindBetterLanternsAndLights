using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BetterLanternsAndLights.BLL_Plugin;

namespace BetterLanternsAndLights
{
    internal class DragonCliffs
    {
        internal static List<GameObject> DCGateLampGOs { get; private set; }

        internal static void Initialize()
        {
            DCGateLampGOs = new List<GameObject>();
            var positions = new Vector3[2]
            {
                new Vector3(0f, 3.35f, 3.1f),
                new Vector3(0f, -3.35f, 3.1f)
            };

            var scenery = GameObject.Find("island 9 E (dragon cliffs) scenery");
            var parent = scenery.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals("east_gate (4)"));
            var lamp = scenery.transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name.Equals("east_street_rope (1)")).GetChild(0);
            foreach (var position in positions)
            {
                var gateLamp = Object.Instantiate(lamp.gameObject, parent);
                gateLamp.transform.localScale = new Vector3(1.33f, 1.33f, 1.33f);
                gateLamp.transform.localPosition = position;
                gateLamp.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                gateLamp.GetComponent<Light>().range = 50f;
                gateLamp.GetComponent<Light>().intensity = 1.5f;
                DCGateLampGOs.Add(gateLamp);
            }
            LogDebug("Dragon Cliffs gate lamps added");
        }
    }
}
