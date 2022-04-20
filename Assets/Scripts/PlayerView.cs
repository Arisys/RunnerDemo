using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] Transform cameraHolder;
    [SerializeField] Transform orientation;

    InputMaster _input;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Awake()
    {
        _input = new InputMaster();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        // rotation aroun x axis
        cameraHolder.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        //rotation aroud y axis
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void MyInput()
    {
        mouseX = _input.Player.Look.ReadValue<Vector2>().x;
        mouseY = _input.Player.Look.ReadValue<Vector2>().y;

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }
}
