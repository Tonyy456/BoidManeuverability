using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By: Taylor Liu
//Took from Lab3
public class InputManager : MonoBehaviour
{
    [SerializeField] private MoveController moveController;
    [SerializeField] private LookController lookController;
    [SerializeField] private ToggleCamera toggleCamera;
    private Player inputScheme;

    //Initialize controller for the player
    private void Awake() {
        inputScheme = new Player();
        lookController.Initialize(inputScheme.PlayerControls.Look);
        moveController.Initialize(inputScheme.PlayerControls.Movement, inputScheme.PlayerControls.Sprint, inputScheme.PlayerControls.Jump);
        toggleCamera.Initialize(inputScheme.PlayerControls.ToggleCamera);
    }
}
