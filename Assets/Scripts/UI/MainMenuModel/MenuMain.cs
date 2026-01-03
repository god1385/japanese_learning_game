using UnityEngine;

public class MenuMain : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MainMenuView mainMenuView;
    [SerializeField] private RectTransform zoomTarget;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float targetCurtainMegePoint;
    [SerializeField] private string nextLevelName;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;

        mainMenuView.Initialize(_camera);
        var model = new MainMenuModel(mainMenuView);
        model.SetData(zoomTarget, zoomAmount, targetCurtainMegePoint, 1f, nextLevelName);
        var presenter = new MainMenuPresenter(model, mainMenuView);
    }
}
