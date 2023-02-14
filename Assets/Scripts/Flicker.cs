using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flicker : MonoBehaviour
{
    public float lightIntensity;
    public float flickerIntensity;

    public float lightTime;
    public float flickerTime;

    System.Random rg;

    Light2D flashlight;

    void Awake()
    {
        rg = new System.Random();
        flashlight = GetComponent<Light2D>();
    }

    void Start()
    {
        StartCoroutine(Flickerstuff());
    }

    IEnumerator Flickerstuff()
    {
        while (true)
        {
            flashlight.intensity = lightIntensity;

            float lightingTime = lightTime + ((float)rg.NextDouble() - 0.5f);
            yield return new WaitForSeconds(lightingTime);

            int flickerCount = rg.Next(4, 9);

            for (int i = 0; i < flickerCount; i++)
            {
                float flickingIntensity = lightIntensity - ((float)rg.NextDouble() * flickerIntensity);
                flashlight.intensity = flickingIntensity;

                float flickingTime = (float)rg.NextDouble() * flickerTime;
                yield return new WaitForSeconds(flickingTime);
            }
        }
    }
}
