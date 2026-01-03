using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuView : ViewModel
{
    public override void DisplayTransitionCurtains(RectTransform zoomTarget, float targetFov, float targetCurtainPosition, float fadeTarget, string nextLevelName = null)
    {
        Sequence seq = DOTween.Sequence();

        // Шторки
        seq.Append(FirstCurtain.DOAnchorPosX(targetCurtainPosition, 0.7f).SetEase(Ease.InOutSine));
        seq.Join(SecondCurtain.DOAnchorPosX(-targetCurtainPosition, 0.7f).SetEase(Ease.InOutSine));
        seq.Join(FirstCurtainImage.DOFade(fadeTarget, 0.7f));
        seq.Join(SecondCurtainImage.DOFade(fadeTarget, 0.7f));
    }
}
