using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerHandInteractable : MonoBehaviour, ISymbolToCollect, IInteractable, ITutorial
{
    [SerializeField] private SymbolData symbolUnlocked;
    [SerializeField] private GameObject outline;
    public SymbolData SymbolToUnlock => symbolUnlocked;

    public Transform InteractionPoint => this.transform;

    [Inject] private SymbolInteractionsConnector _connector;

    private bool _canInteract = false;
    private TutorialPresenter _tutorial;

    public event Action OnInteracted;


    public void Interact()
    {
        if (_canInteract)
        {
            OnUnfocus();
            OnInteracted?.Invoke();
        }
    }

    public void OnFocus()
    {
        if (_canInteract)
        {
            outline.SetActive(true);
        }
    }

    public void OnUnfocus()
    {
        if (_canInteract)
        {
            outline.SetActive(false);
        }
    }

    public void EnableInteraction(bool enabled)
    {
        _canInteract = enabled;
        gameObject.SetActive(enabled);
    }

    public void CollectSymbol()
    {
        _connector.CollectSymbol(this);
    }

    public Task PlayAnimationAsync(List<Sprite> frames)
    {
        throw new NotImplementedException();
    }
}
