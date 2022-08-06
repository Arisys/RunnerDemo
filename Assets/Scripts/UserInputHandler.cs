using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// this class takes the user Input and calls the coresponding functions within the player character
/// </summary>
public class UserInputHandler : MonoBehaviour
{
    //Input Manager
    InputMaster _input;

    private void Awake()
    {
        _input = new InputMaster();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    private void Update()
    {
        _input.Player.Jump.performed += Jump;

    }
}
