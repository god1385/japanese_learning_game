using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LevelLightningHandler : MonoBehaviour
{
    [SerializeField] private List<LightningIdentification> listOfLightData;
    [SerializeField] private float fadeDuration;

    public async Task OnTutorialStepChanged(string stepIndex)
    {
        foreach (var lightningIdentification in listOfLightData)
        {
            if (lightningIdentification.lightIndex == stepIndex)
                await FadeLightsAsync(lightningIdentification.lightInstances);
        }
    }

    public async Task FadeLightsAsync(List<LightningInstance> lights)
    {
        List<Tween> tweens = new List<Tween>();

        foreach (var light in lights)
        {
            if (light.light == null) continue;
            tweens.Add(light.light.DOIntensity(light.lightTargerIntensity, fadeDuration).SetEase(Ease.InOutSine));
        }

        await Task.WhenAll(tweens.Select(t => t.AsyncWaitForCompletion()));
    }
}

[System.Serializable]
public struct LightningInstance
{
    public Light light;
    public float lightTargerIntensity;
}

[System.Serializable]
public struct LightningIdentification
{
    public List<LightningInstance> lightInstances;
    public string lightIndex;
}
