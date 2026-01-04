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

        void Awake()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            InputSystem.actions.FindAction("Interact").performed += _ => Interact?.Invoke();
        }

        void FixedUpdate()
        {
            moveDirection = moveAction.ReadValue<Vector2>();
            Move?.Invoke(moveDirection);
        }
    }
}
