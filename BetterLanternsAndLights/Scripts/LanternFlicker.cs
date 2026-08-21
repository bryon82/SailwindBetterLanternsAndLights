using System.Collections;
using UnityEngine;
using static BetterLanternsAndLights.Configs;

namespace BetterLanternsAndLights
{
    internal class LanternFlicker : MonoBehaviour
    {
        private Coroutine flickerCoroutine;
        private Material paperMaterial;
        private Material paperOnMat;
        private Material paperOffMat;
        private Light light;
        private Color baseEmission;
        private float minIntensity = 1f;
        private float maxIntensity = 1.5f;
        private float minEmission = 0.5f;
        private float maxEmission = 0.8f;

        private void Awake()
        {
            light = GetComponentInChildren<Light>();
            maxIntensity = light.intensity;
            minIntensity = light.intensity - 0.5f;

            var shipItemLight = GetComponent<ShipItemLight>();
            Renderer paperRenderer = null;
            if (shipItemLight != null)
            {
                paperRenderer = shipItemLight.GetPrivateField<Renderer>("paperRenderer");
                paperOffMat = shipItemLight.GetPrivateField<Material>("paperOffMat");
                paperOnMat = paperRenderer?.sharedMaterial;
            }

            var islandStreetlight = GetComponent<IslandStreetlight>();
            if (islandStreetlight != null)
            {
                paperRenderer = islandStreetlight.GetPrivateField<Renderer>("renderer");
                paperOffMat = islandStreetlight.GetPrivateField<Material>("offMat");
                paperOnMat = paperRenderer?.sharedMaterial;
            }

            if (paperRenderer != null)
                paperMaterial = paperRenderer.material;

            if (paperOnMat != null)
                baseEmission = paperOnMat.GetColor("_EmissionColor");
        }

        internal void SetFlicker(bool enable)
        {
            if (enableLanternFlicker.Value && enable && flickerCoroutine == null)
            {
                flickerCoroutine = StartCoroutine(Flicker());
            }
            else if (!enable && flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
                flickerCoroutine = null;
            }
        }

        private IEnumerator Flicker()
        {
            float slowTime = Random.value * 100f;
            float fastTime = Random.value * 100f;

            while (true)
            {
                slowTime += Time.deltaTime * 0.5f;
                fastTime += Time.deltaTime * 6f;

                float slow = Mathf.PerlinNoise(slowTime, 0f);
                float fast = Mathf.PerlinNoise(fastTime, 100f);
                float flicker = slow * 0.8f + fast * 0.2f;
                flicker = Mathf.Clamp01(flicker);

                light.intensity = Mathf.Lerp(
                    minIntensity,
                    maxIntensity,
                    flicker
                );

                if (paperOffMat == null)
                {
                    yield return null;
                    continue;
                }

                float materialAmount = Mathf.Lerp(0.9f, 1.0f, flicker);
                paperMaterial.Lerp(
                    paperOffMat,
                    paperOnMat,
                    materialAmount
                );

                float emissionStrength = Mathf.Lerp(
                    minEmission,
                    maxEmission,
                    flicker
                );

                paperMaterial.SetColor(
                    "_EmissionColor",
                    baseEmission * emissionStrength
                );

                yield return null;
            }
        }
    }
}
