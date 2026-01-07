using UnityEngine;

public class PlayerInputBlocker : MonoBehaviour, IInputBlocker
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteractionWithCollectibles interaction;
    public void Block()
    {
        movement.SetEnabledValue(false);
        interaction.SetEnabledValue(false);
    }
    public void UnBlock()
    {
        movement.SetEnabledValue(true);
        interaction.SetEnabledValue(true);
    }
}
