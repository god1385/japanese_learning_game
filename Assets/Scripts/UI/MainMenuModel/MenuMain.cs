using UnityEngine;

public class MenuMain : MonoBehaviour
{
    [SerializeField] private MainMenuView mainMenuView;
    [SerializeField] private RectTransform zoomTarget;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float targetCurtainMegePoint;
    [SerializeField] private string nextLevelName;

    private void Awake()
    {
        var model = new MainMenuModel(mainMenuView);
        model.SetData(zoomTarget, zoomAmount, targetCurtainMegePoint, 1f, nextLevelName);
        var presenter = new MainMenuPresenter(model, mainMenuView);
    }
}
