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

    private void OnEnable()
    {
        LightController.LightController.OnLightAdded += OnLightAdded;
    }

    private void OnDisable()
    {
        LightController.LightController.OnLightAdded -= OnLightAdded;
    }
}
