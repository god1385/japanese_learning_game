using System;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialNarrator : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private float charDelay = 0.03f;


    public async Task Play(string text)
    {
        // текст / VO / тайпинг
        await SayAsync(text);
    }

    public async Task SayAsync(string message)
    {
        text.text = "";

        foreach (var c in message)
        {
            text.text += c;
            await Task.Delay(TimeSpan.FromSeconds(charDelay));
        }

        // Пауза после текста
        await Task.Delay(500);
    }
}
