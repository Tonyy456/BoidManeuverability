using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Written By: Taylor Liu
public class ToggleCamera : MonoBehaviour
{
    [SerializeField] List<Camera> cams;

    private InputAction toggleCamera;
    private int curCam;

    public void Initialize(InputAction toggleCamera) {

        for (int i = 0; i < cams.Count; i++) {
            cams[i].enabled = false;
        }

        curCam = 0;
        cams[curCam].enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        this.toggleCamera = toggleCamera;
        toggleCamera.performed += Toggle;
        toggleCamera.Enable();
    }

    private void Toggle(InputAction.CallbackContext e) {
        var nexCam = (curCam + 1) % cams.Count;
        cams[curCam].enabled = false;
        cams[curCam].GetComponent<AudioListener>().enabled = false;
        if(cams[curCam].GetComponent<LookController>() != null) {
            cams[curCam].GetComponent<LookController>().enabled = false;
        }
        cams[nexCam].enabled = true;
        cams[nexCam].GetComponent<AudioListener>().enabled = true;
        if (cams[nexCam].GetComponent<LookController>() != null) {
            cams[nexCam].GetComponent<LookController>().enabled = true;
        }
        curCam = nexCam;
    }
}
