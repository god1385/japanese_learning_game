using System;
using UnityEngine;
using Zenject;

public class CollectibleSymbol : MonoBehaviour, ISymbolToCollect, IInteractable
{
    [SerializeField] private SymbolData symbolInfo;
    [SerializeField] private Transform interactablePoint;
    [SerializeField] private GameObject outline;

    [Inject] private SymbolInteractionsConnector _connector;
    public SymbolData SymbolToUnlock => symbolInfo;

    public Transform InteractionPoint => interactablePoint;


    private bool _interacted = false;

    public event Action OnInteracted;

    public void Interact()
    {
        if (_interacted) return;

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

    public void CollectSymbol()
    {
        _connector.CollectSymbol(this);
    }
}
