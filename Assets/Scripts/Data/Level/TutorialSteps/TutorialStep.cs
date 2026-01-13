using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "LevelData/TutorialStep")]
public class TutorialStep : ScriptableObject
{
    public string stepId;
    public GameObject interactableObject;
    public string animationId;
    public List<NarratorText> narratorText;
    public List<NarratorText> narratorTextIfWaitForInteraction;
    public float delayBeforeNextStep = 0f;
    public bool canCollectSymbol = false;
    public bool canInteractAfterStep = false;
    public bool isStepEndingWithShake = false;
    public bool isChangingLightning = false;
    public List<StepActionType> actionsOrder;
}
