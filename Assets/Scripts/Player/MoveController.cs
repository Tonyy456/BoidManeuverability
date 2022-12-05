using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour {
    [SerializeField] public Transform targetToMove;
    private Transform targetMoveTowards;
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private float speedMul = 2f;
    [SerializeField] private float jumpHeight = 20f;

    private float xMove, yMove;
    private InputAction move;
    private InputAction sprint;
    private InputAction jump;
    private bool sprinting = false;

    //Initialize the movement controller
    public void Initialize(InputAction move, InputAction sprint, InputAction jump) {
        targetMoveTowards = GetComponent<Transform>().transform;
        this.sprint = sprint;
        sprint.performed += ToggleSprint;
        this.move = move;
        this.jump = jump;
        jump.performed += Jump;
        sprint.Enable();
        move.Enable();
        jump.Enable();
    }

    //Callbackcontext that toggles speed for sprinting
    private void ToggleSprint(InputAction.CallbackContext e) {
        sprinting = !sprinting;
        if (sprinting) {
            speed *= speedMul;
        }
        else {
            speed /= speedMul;
        }
    }

    //Callbackcontext that make the player jump
    private void Jump(InputAction.CallbackContext e) {
        targetMoveTowards.position = jumpHeight * targetToMove.up;

        if (Physics.Raycast(targetToMove.position, targetToMove.up * -1, 1f))
            targetToMove.GetComponent<Rigidbody>().AddForce(targetMoveTowards.position);
    }

    //Movees the player at every fixed update towards the direction that the keyboard indicates
    void FixedUpdate() {
        xMove = move.ReadValue<Vector2>().x;
        yMove = move.ReadValue<Vector2>().y;

        targetMoveTowards.position = xMove * targetToMove.transform.right + yMove * targetToMove.transform.forward;

        targetToMove.position = Vector3.MoveTowards(targetToMove.position, targetToMove.position + targetMoveTowards.position, speed);
    }
}
