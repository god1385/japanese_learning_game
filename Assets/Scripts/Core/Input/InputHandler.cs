using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class InputHandler : MonoBehaviour
    {
        private InputAction moveAction;
        private InputAction interactAction;
        static private Vector2 moveDirection;
        public static event Action <Vector2> Move;
        public static event Action Interact;
        public static event Action InteractWithMouth;
        public static Vector2 mousePosition;

        void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            InputSystem.actions.FindAction("Interact").performed += _ => Interact?.Invoke();
            InputSystem.actions.FindAction("MouseInteract").performed += _ => InteractWithMouth?.Invoke();
            
        }

        void FixedUpdate()
        {
            mousePosition = Mouse.current.position.ReadValue();
            moveDirection = moveAction.ReadValue<Vector2>();
            Move?.Invoke(moveDirection);
        }
    }
}
