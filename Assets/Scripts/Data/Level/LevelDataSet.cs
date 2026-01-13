using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataSet : MonoBehaviour
{
    [SerializeField] private List<SpritesForAnimation> playerSprites;
    [SerializeField] private List<TutorialStep> stepData;
    [SerializeField] private List<GameObject> gameObjectsToLink;
    [SerializeField] private List<WordData> words;
    [SerializeField] private List<NarratorText> tutorialInitialText;
    [SerializeField] private List<CanvasGroup> uiElementsToFadeInTheEnd;

    public List<NarratorText> TutorialInitialText => tutorialInitialText;
    public List<CanvasGroup> UiElementsToFadeInTheEnd => uiElementsToFadeInTheEnd;
    public IReadOnlyList<WordData> Words => words;

    public IReadOnlyList<TutorialStep> StepData => stepData;

    private void Awake()
    {
        for (int i = 0; i < stepData.Count; i++)
        {
            if (i < gameObjectsToLink.Count)
                stepData[i].interactableObject = gameObjectsToLink[i];
        }
    }
    public TutorialStep ReturnRequiredStep(int index)
    {
        if (index >= stepData.Count) return null;
        else return stepData[index];
    }
    public List<Sprite> ReturnRequiredSprites(string id)
    {
        foreach (var obj in playerSprites)
        {
            if (obj.nameId == id)
                return obj.sprites;
        }

        return null;
    }
}

[System.Serializable]
public struct SpritesForAnimation
{
    public string nameId;
    public List<Sprite> sprites;
}

[System.Serializable]
public class NarratorText
{
    [TextArea]
    public string text;
    public float delayAfter = 0.5f;
}

[System.Serializable]
public enum StepActionType
{
    WaitForInteraction,
    NarratorText,
    CollectSymbol,
    PlayAnimation,
    PlayShake,
    ChangeLightning,
    FinishTutorial
}
