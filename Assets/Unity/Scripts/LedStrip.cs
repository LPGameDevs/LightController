using System;
using UnityEngine;
using Light = LightController.Light;

public class LedStrip : MonoBehaviour
{
    public LedLight LightPrefab;

    private void OnLightAdded(Light light)
    {
        var led = Instantiate(LightPrefab, transform);
        led.SetLight(light);
    }

    private void OnLightRemoved(Light light)
    {
        var leds = GetComponentsInChildren<LedLight>();
        for (int i = 0; i < leds.Length; i++)
        {
            if (leds[i].GetPosition() == light.GetPosition())
            {
                Destroy(leds[i].gameObject);
                return;
            }
        }
    }

    private void OnEnable()
    {
        LightController.LightController.OnLightAdded += OnLightAdded;
        LightController.LightController.OnLightRemoved += OnLightRemoved;
    }

    private void OnDisable()
    {
        LightController.LightController.OnLightAdded -= OnLightAdded;
        LightController.LightController.OnLightRemoved -= OnLightRemoved;
    }
}
