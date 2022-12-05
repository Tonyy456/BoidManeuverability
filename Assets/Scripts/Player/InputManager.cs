using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private MoveController moveController;
    [SerializeField] private LookController lookController;
    private Player inputScheme;

    //Initialize controller for the player
    private void Awake() {
        inputScheme = new Player();
        lookController.Initialize(inputScheme.PlayerControls.Look);
        moveController.Initialize(inputScheme.PlayerControls.Movement, inputScheme.PlayerControls.Sprint, inputScheme.PlayerControls.Jump);
    }
}
