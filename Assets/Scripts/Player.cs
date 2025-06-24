using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private PlayerInput playerInput;

    private bool isMoving;
    private string arrowPressed;

    private Vector3 currPosition;

    private void Awake() {
        currPosition = new Vector3(0, 0, 0);
        transform.position = currPosition;
    }

    private void Start() {
        playerInput.OnArrowPressed += PlayerInput_OnArrowPressed; ;
    }

    private void PlayerInput_OnArrowPressed(object sender, PlayerInput.ArrowPressedEventArgs e) {
        string keyPressed = e.ArrowKey;

        switch (keyPressed) {
            case "rightArrow":
                if(transform.position.x == 1) {
                    transform.position = new Vector3(2.5f, 0, 0);
                }
                break;
            case "leftArrow":
                if (transform.position.x == 3.5f) {
                    transform.position = new Vector3(0, 0, 0);
                }
                break;
        }
    }

    private void Update() {
        Vector2 inputVector = playerInput.GetNormMovementVector();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        //transform.position += moveDir * moveSpeed * Time.deltaTime;

        isMoving = moveDir != Vector3.zero;

    }
}
