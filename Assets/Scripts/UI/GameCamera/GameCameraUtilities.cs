using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class GameCameraUtilities : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vcam;

    [Header("Tutorial Shake")]
    [SerializeField] private float amplitude = 0.3f;
    [SerializeField] private float frequency = 1.2f;
    [SerializeField] private float duration = 0.15f;

    private CinemachineBasicMultiChannelPerlin _noise;

    private Tween _shakeTween;

    private void Awake()
    {
        _noise = vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        _noise.AmplitudeGain = 0f;
    }

    private void Start()
    {
    }

    public void PlayTutorialShake()
    {
        _shakeTween?.Kill();

        _noise.FrequencyGain = frequency;

        _shakeTween = DOTween.To(
                () => _noise.AmplitudeGain,
                x => _noise.AmplitudeGain = x,
                amplitude,
                duration * 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                DOTween.To(
                    () => _noise.AmplitudeGain,
                    x => _noise.AmplitudeGain = x,
                    0f,
                    duration * 0.7f);
            });
    }
}
