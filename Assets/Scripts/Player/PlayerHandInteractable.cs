using UnityEngine;
using Zenject;

public class PlayerHandInteractable : MonoBehaviour, ISymbolToCollect, IInteractable
{
    [SerializeField] private SymbolData symbolUnlocked;
    [SerializeField] private GameObject outline;
    public SymbolData SymbolToUnlock => symbolUnlocked;

    public Transform InteractionPoint => this.transform;

    [Inject] private TutorialInfo _tutorialInfo;
    [Inject] private SymbolInteractionsConnector _connector;

    private bool _interacted = false;

    public void Interact()
    {
        if (_tutorialInfo.isMosquitoClicked && !_tutorialInfo.isHandClicked)
        {
            if (_interacted) return;

            _interacted = true;
            _tutorialInfo.isHandClicked = true;

            _connector.CollectSymbol(this);

            gameObject.SetActive(false);
        }
    }

    public void OnFocus()
    {
        if (_tutorialInfo.isMosquitoClicked && !_tutorialInfo.isHandClicked)
        {
            if (!_interacted)
                outline.SetActive(true);
        }
    }

    public void OnUnfocus()
    {
        if (_tutorialInfo.isMosquitoClicked && !_tutorialInfo.isHandClicked)
        {
            outline.SetActive(false);
        }
    }
}
