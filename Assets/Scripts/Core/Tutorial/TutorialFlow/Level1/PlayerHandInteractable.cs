using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerHandInteractable : MonoBehaviour, ISymbolToCollect, IInteractable, ITutorial
{
    [SerializeField] private List<SymbolData> symbolUnlocked;
    [SerializeField] private GameObject outline;

    private bool _canInteract = false;
    public event Action OnInteracted;

    [Inject] private SymbolInteractionsConnector _connector;
    public IReadOnlyList<SymbolData> SymbolsToUnlock => symbolUnlocked;
    public Transform InteractionPoint => this.transform;




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
        if (_canInteract && outline != null)
        {
            outline.SetActive(true);
        }
    }

    public void OnUnfocus()
    {
        if (_canInteract && outline != null)
        {
            outline.SetActive(false);
        }
    }

    public void EnableInteraction(bool enabled)
    {
        _canInteract = enabled;
        gameObject.SetActive(enabled);
    }

    public async Task CollectSymbol()
    {
        await _connector.CollectSymbol(this);
    }

    public Task PlayAnimationAsync(List<Sprite> frames)
    {
        return Task.CompletedTask;
    }
}
