using static Model;

public abstract class Presenter<TModel, TView> 
    where TModel : Model
    where TView : ViewModel
{
    protected TView _viewModel;
    protected TModel _model;
    public Presenter(TModel model, TView viewModel)
    {
        _model = model;
        _viewModel = viewModel;
        _model.OnStateChanged += OnStateChanged;
    }

    public void OnStateChanged(MenuState state)
    {
        if (state == MenuState.Transition)
        {
            _viewModel.DisplayTransitionCurtains(_model.CurrentZoomTarget, _model.ZoomAmount, _model.TargetCurtainPosition, _model.FadeTarget, _model.NextLevelString);
        }
    }
}
