using UnityEngine;

public abstract class Presenter
{
    public ViewModel _viewModel;
    public Model _model;
    public Presenter(Model model, ViewModel viewModel)
    {
        _model = model;
        _viewModel = viewModel;
    }
}
