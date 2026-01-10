using System.Collections.Generic;
using UnityEngine;

public class LevelDataSet : MonoBehaviour
{
    [SerializeField] private List<SpritesForAnimation> playerSprites;
    [SerializeField] private List<TutorialStepData> stepData;

    public List<TutorialStepData> StepData => stepData;
    public TutorialStepData ReturnRequiredStep(int index)
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
public class TutorialStepData
{
    public string stepId;
    public GameObject interactableObject;
    public string animationId;
    public string narratorText;
    public float delayBeforeNextStep = 0f;
    public bool canInteractAfterStep = false;
}
