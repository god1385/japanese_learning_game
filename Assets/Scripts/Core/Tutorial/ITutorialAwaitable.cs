using System;
using System.Threading.Tasks;
using UnityEngine;

public interface ITutorialAwaitable
{
    public Task WaitForCompletionAsync();
    void SetActionAfterInteraction(Func<Task> asyncAction);
}
