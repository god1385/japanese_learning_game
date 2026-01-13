using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ScrollInterectable : MonoBehaviour, IInteractable, ITutorial, ITutorialAwaitable
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private GameObject outline;
    [SerializeField] private ScrollDragAndDropMiniGame minigame;

    private bool _canInteract = false;
    public Transform InteractionPoint => interactionPoint;
    private TaskCompletionSource<bool> _completionSource;
    private Func<Task> _asyncAction;

    public event Action OnInteracted;

    public void EnableInteraction(bool enabled)
    {
        _canInteract = enabled;
    }

    public async void Interact()
    {
        if (!_canInteract) return;

        OnInteracted.Invoke();
        await minigame.Initialize(CompleteMiniGame, _asyncAction);
        OnUnfocus();
        _canInteract = false;
    }

    public void OnFocus()
    {
        if (!_canInteract) return;

        outline.SetActive(true);
    }

    public void OnUnfocus()
    {
        outline.SetActive(false);
    }

    public Task PlayAnimationAsync(List<Sprite> frames)
    {
        throw new NotImplementedException();
    }
    public Task WaitForCompletionAsync()
    {
        _completionSource = new TaskCompletionSource<bool>();
        return _completionSource.Task;
    }

    private void CompleteMiniGame()
    {
        _completionSource.TrySetResult(true);
    }

    void ITutorialAwaitable.SetActionAfterInteraction(Func<Task> asyncAction)
    {
        _asyncAction = asyncAction;
    }
}
