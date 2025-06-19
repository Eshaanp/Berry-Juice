using UnityEngine;

public class PlayerInput : MonoBehaviour {

    private GameInputActions gameInputActions;

    private void Awake() {
        gameInputActions = new GameInputActions();
        gameInputActions.PlayerActions.Enable();
    }

    public Vector2 GetNormMovementVector() {
        Vector2 inputVector = gameInputActions.PlayerActions.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
