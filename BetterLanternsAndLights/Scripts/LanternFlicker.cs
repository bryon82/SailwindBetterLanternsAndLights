using System.Collections;
using UnityEngine;
using static BetterLanternsAndLights.Configs;

namespace BetterLanternsAndLights
{
    internal class LanternFlicker : MonoBehaviour
    {
        private bool isLantern = false;
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
        private float minWindSpeed = 20f;
        private float maxWindSpeed = 40f;
        private float minFlickerSpeed = 0.5f;
        private float maxFlickerSpeed = 2f;
        private float minFastSpeed = 6f;
        private float maxFastSpeed = 15f;
        private float minFastAmount = 0.2f;
        private float maxFastAmount = 0.5f;

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
                isLantern = true;
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
            var slowTime = Random.value * 100f;
            var fastTime = Random.value * 100f;

            while (true)
            {
                var windSpeed = Wind.currentWind.magnitude;
                if (GameState.indoors || !isLantern)
                    windSpeed = 0f;

                var windAmount = Mathf.InverseLerp(minWindSpeed, maxWindSpeed, windSpeed);

                var slowSpeed = Mathf.Lerp(minFlickerSpeed, maxFlickerSpeed, windAmount);
                var fastSpeed = Mathf.Lerp(minFastSpeed, maxFastSpeed, windAmount);
                slowTime += Time.deltaTime * slowSpeed;
                fastTime += Time.deltaTime * fastSpeed;
                var slow = Mathf.PerlinNoise(slowTime, 0f);
                var fast = Mathf.PerlinNoise(fastTime, 100f);

                var fastAmount = Mathf.Lerp(minFastAmount, maxFastAmount, windAmount);
                var slowAmount = 1f - fastAmount;

                var flicker = slow * slowAmount + fast * fastAmount;
                flicker = Mathf.Clamp01(flicker);

                light.intensity = Mathf.Lerp(minIntensity, maxIntensity, flicker);

                if (paperOffMat == null)
                {
                    yield return null;
                    continue;
                }

                var materialAmount = Mathf.Lerp(0.9f, 1.0f, flicker);
                paperMaterial.Lerp(paperOffMat, paperOnMat, materialAmount);

                var emissionStrength = Mathf.Lerp(minEmission, maxEmission, flicker);
                paperMaterial.SetColor("_EmissionColor", baseEmission * emissionStrength);

                yield return null;
            }
        }
    }
}
