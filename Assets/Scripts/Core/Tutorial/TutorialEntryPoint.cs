using UnityEngine;
using Zenject;

public class TutorialEntryPoint : MonoBehaviour
{
    private TutorialPresenter _tutorial;

    [Inject]
    public void Construct(TutorialPresenter tutorial)
    {
        _tutorial = tutorial;
    }

    private void Start()
    {
        _tutorial.StartTutorial();
    }
}
