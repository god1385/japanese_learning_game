using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : ViewModel
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private MainMenuPresenter presenter; 

    public Button PlayButton => playButton;
    public Button SettingsButton => settingsButton;
    public Button ExitButton => exitButton;
    private Camera _camera;

    public void Initialize(Camera camera)
    {
        _camera = camera;
    }

    public void AssignAction(Button button, UnityAction action)
    {
        button?.onClick.AddListener(action);
    }

    public override void DisplayTransitionCurtains(RectTransform zoomTarget, float targetFov, float targetCurtainPosition, float fadeTarget, string nextLevelName = null)
    {
        Sequence seq = DOTween.Sequence();

        // Камера зум и движение
        seq.Append(_camera.transform.DOMove(zoomTarget.position, 1.5f).SetEase(Ease.InOutSine));


        // Шторки
        seq.Append(FirstCurtain.DOAnchorPosX(targetCurtainPosition, 0.7f).SetEase(Ease.InOutSine));
        seq.Join(SecondCurtain.DOAnchorPosX(-targetCurtainPosition, 0.7f).SetEase(Ease.InOutSine));
        seq.Join(FirstCurtainImage.DOFade(fadeTarget, 0.7f));
        seq.Join(SecondCurtainImage.DOFade(fadeTarget, 0.7f));
        // После окончания

        if (nextLevelName != null)
            seq.OnComplete(() => SceneManager.LoadSceneAsync(nextLevelName));
    }

    private void OnDestroy()
    {
        playButton?.onClick.RemoveAllListeners();
        settingsButton?.onClick.RemoveAllListeners();
        exitButton?.onClick.RemoveAllListeners();
    }
}
