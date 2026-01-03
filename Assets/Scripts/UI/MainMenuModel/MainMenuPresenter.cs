using UnityEngine;

public class MainMenuPresenter : Presenter<MainMenuModel, MainMenuView>
{
    public MainMenuPresenter(MainMenuModel model, MainMenuView mainMenuView) : base(model, mainMenuView)
    {
        mainMenuView.AssignAction(mainMenuView.PlayButton, OnPlayClicked);
        mainMenuView.AssignAction(mainMenuView.SettingsButton, OnSettingsClicked);
        mainMenuView.AssignAction(mainMenuView.ExitButton, OnExitClicked);
    }
    private void OnPlayClicked()
    {
        Debug.Log("Start game");
        _model.SetState(Model.MenuState.Transition);
    }

    private void OnSettingsClicked()
    {
        _model.SetState(Model.MenuState.Settings);
        Debug.Log("Settings");
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }
}
