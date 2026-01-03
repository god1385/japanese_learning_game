using System;
using UnityEngine;

public abstract class Model
{
    public enum MenuState { Main, Settings, Transition }
    public MenuState CurrentState { get; private set; } = MenuState.Main;
    public RectTransform CurrentZoomTarget { get; private set; }
    public float ZoomAmount { get; private set; } = 2f;
    public float TargetCurtainPosition { get; private set; } = 0f;
    public float FadeTarget {  get; private set; } = 1f;
    public string NextLevelString { get; private set; }

    public event Action<MenuState> OnStateChanged;
    private ViewModel _view;

    public Model(ViewModel view)
    {
        _view = view;
    }

    public void SetData(RectTransform zoomTarget, float zoomAmount, float targetCurtainAmount, float fadeTarget, string nextLevelString = null)
    {
        CurrentZoomTarget = zoomTarget;
        ZoomAmount = zoomAmount;
        TargetCurtainPosition = targetCurtainAmount;
        FadeTarget = fadeTarget;
        NextLevelString = nextLevelString;
    }

    public void SetState(MenuState state)
    {
        CurrentState = state;
        OnStateChanged?.Invoke(state);
    }
}
