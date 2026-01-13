using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CollectibleSymbol : MonoBehaviour, ISymbolToCollect, IInteractable
{
    [SerializeField] private List<SymbolData> symbolInfo;
    [SerializeField] private Transform interactablePoint;
    [SerializeField] private GameObject outline;

    [Inject] private SymbolInteractionsConnector _connector;
    public IReadOnlyList<SymbolData> SymbolsToUnlock => symbolInfo;

    public Transform InteractionPoint => interactablePoint;


    private bool _interacted = false;

    public event Action OnInteracted;

    public void Interact()
    {
        if (_interacted) return;

        OnInteracted.Invoke();
        _interacted = true;

        gameObject.SetActive(false);
    }

    public void OnFocus()
    {
        if (!_interacted)
            outline.SetActive(true);
    }

    public void OnUnfocus()
    {
        outline.SetActive(false);
    }

    public async Task CollectSymbol()
    {
        await _connector.CollectSymbol(this);
    }
}
