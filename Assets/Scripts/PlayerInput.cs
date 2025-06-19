using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerInput : MonoBehaviour {

    private GameInputActions gameInputActions;

    public event EventHandler<ArrowPressedEventArgs> OnArrowPressed;

    private void Awake() {
        gameInputActions = new GameInputActions();
        gameInputActions.PlayerActions.Enable();

        gameInputActions.PlayerActions.ArrowKeys.performed += Arrow_Pressed;
    }

    public void Arrow_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        string keyName = obj.control.name;
        OnArrowPressed?.Invoke(this, new ArrowPressedEventArgs { ArrowKey = keyName });
    }

    public Vector2 GetNormMovementVector() {
        Vector2 inputVector = gameInputActions.PlayerActions.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public class ArrowPressedEventArgs : EventArgs {
        public string ArrowKey { get; set; }
    }
}
