using UnityEngine;

public class PlayerInputBlocker : MonoBehaviour, IInputBlocker
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteractionWithCollectibles interaction;
    public void Block()
    {
        movement.EnableInteraction(false);
        interaction.SetEnabledValue(false);
    }
    public void UnBlock()
    {
        movement.EnableInteraction(true);
        interaction.SetEnabledValue(true);
    }
}
