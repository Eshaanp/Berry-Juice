using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private PlayerInput playerInput;

    private bool isMoving;

    private void Update() {
        Vector2 inputVector = playerInput.GetNormMovementVector();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isMoving = moveDir != Vector3.zero;
    }
}
