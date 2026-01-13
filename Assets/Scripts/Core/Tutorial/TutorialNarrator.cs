using Game.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class TutorialNarrator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float charDelay = 0.03f;

    public async Task PlaySequence(List<NarratorText> lines)
    {
        // текст / VO / тайпинг
        text.gameObject.SetActive(true);
        await PlayLines(lines);
    }

    public async Task PlayLines(List<NarratorText> lines)
    {
        foreach (var line in lines)
        {
            if (line.text != null)
            {
                await SayAsync(line.text);
                await WaitAfterLine(line.delayAfter);
            }
        }

        text.gameObject.SetActive(false);
    }

    public async Task SayAsync(string message)
    {
        text.text = "";

        foreach (var c in message)
        {
            text.text += c;
            await Task.Delay(TimeSpan.FromSeconds(charDelay));
        }
    }

    private async Task WaitAfterLine(float delay)
    {
        var delayTask = Task.Delay(TimeSpan.FromSeconds(delay));
        var clickTask = WaitForClickAsync();

        await Task.WhenAny(delayTask, clickTask);
    }

    private Task WaitForClickAsync()
    {
        var tcs = new TaskCompletionSource<bool>();

        void Handler()
        {
            InputHandler.InteractWithMouth -= Handler;
            tcs.TrySetResult(true);
        }

        InputHandler.InteractWithMouth += Handler;
        return tcs.Task;
    }
}
