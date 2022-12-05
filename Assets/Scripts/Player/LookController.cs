using UnityEngine;
using UnityEngine.InputSystem;

//Written By: Taylor Liu
//Took from Lab3
public class LookController : MonoBehaviour {

    [SerializeField] public Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float limitUp = 45f;
    [SerializeField] private float xSens = 2f;
    [SerializeField] private float ySens = 2f;

    private float mouseX = 0f, mouseY = 0f;
    private float xRotation, yRotation;

    private InputAction deltaMouse;

    public void Initialize(InputAction deltaMouse) {
        Cursor.lockState = CursorLockMode.Locked;
        this.deltaMouse = deltaMouse;
        deltaMouse.Enable();
    }

    // Update is called once per frame
    private void Update() {
        //follow on a set position
        transform.position = target.transform.position + offset;

        //rotation of camera and player body
        mouseX = deltaMouse.ReadValue<Vector2>().x * Time.deltaTime;
        mouseY = deltaMouse.ReadValue<Vector2>().y * Time.deltaTime;

        xRotation += mouseX * xSens;
        yRotation -= mouseY * ySens;

        //Limit how far up and down you can look
        yRotation = Mathf.Clamp(yRotation, -limitUp, limitUp);

        var cameraTarget = Quaternion.Euler(new Vector3(yRotation, xRotation));
        var bodyTarget = Quaternion.Euler(new Vector3(0, xRotation));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraTarget, speed);
        target.rotation = Quaternion.RotateTowards(target.rotation, bodyTarget, speed);
    }
}
