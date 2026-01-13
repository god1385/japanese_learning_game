using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LevelLightningHandler : MonoBehaviour
{
    [SerializeField] private List<LightningIdentification> listOfLightData;

    private Dictionary<string, List<LightningInstance>> _stepToLights;

    private void Awake()
    {
        _stepToLights = listOfLightData.ToDictionary(x => x.lightIndex, x => x.lightInstances);
    }

    public async Task OnTutorialStepChanged(string stepIndex)
    {
        if (!_stepToLights.TryGetValue(stepIndex, out var lights)) return;
        await FadeLightsAsync(lights);
    }

    public async Task FadeLightsAsync(List<LightningInstance> lights)
    {
        if (lights == null || lights.Count == 0) return;
        List<Tween> tweens = new List<Tween>();

        foreach (var light in lights)
        {
            if (light.light == null) continue;
            tweens.Add(light.light.DOIntensity(light.lightTargerIntensity, light.fadeDuration).SetEase(Ease.InOutSine));
        }

        await Task.WhenAll(tweens.Select(t => t.AsyncWaitForCompletion()));
    }
}

[System.Serializable]
public struct LightningInstance
{
    public Light light;
    public float lightTargerIntensity;
    public float fadeDuration;
}

[System.Serializable]
public struct LightningIdentification
{
    public List<LightningInstance> lightInstances;
    public string lightIndex;
}
