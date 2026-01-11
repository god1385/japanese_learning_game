using UnityEngine;
using Zenject;

public class TutorialEntryPoint : MonoBehaviour
{
    private TutorialPresenter _tutorial;
    private RoadMapPresenter _roadMap;

    [Inject]
    public void Construct(TutorialPresenter tutorial, RoadMapPresenter roadMap)
    {
        _tutorial = tutorial;
        _roadMap = roadMap;
    }

    private async void Start()
    {
        await _tutorial.StartTutorial();
    }
}
