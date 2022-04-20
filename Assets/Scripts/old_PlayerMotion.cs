using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMotion : MonoBehaviour
{

    [Tooltip("Is the current used input device a mouse?")]
    public bool IsCurrentDeviceMouse;// => _input.controlScheme  == "KeyboardMouse";


    [Header("Movement")]
    [Tooltip("Mouse Sensitivity")]
    public float MouseSensitivity = 0.5f;
    [Tooltip("Mouse movement theshold")]
    public float MouseMoveThreshold = 0.01f;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    // coltrol objectss
    private InputMaster _input;
    public CharacterController _controller;


        float _cinemachineTargetPitch =0f;
        float _rotationVelocity;
    
    // Helper
    // threshold for float checks
    private float _threshold;

    public float SprintSpeed = 20f;
    public float MoveSpeed = 10f;

    public float SpeedChangeRate = 1f;

    private void OnEnable()
    {
        _input.Enable();
    }
    private void Awake()
    {
        _input = new InputMaster();
    }

    // Start is called before the first frame update
    void Start()
    {
        // get object properties on startup
        _input = GetComponent<InputMaster>();

        // findet Objekt nicht ...
        //_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        registerMovement();
        Vector2 mouseMove = _input.Player.Look.ReadValue<Vector2>();
        Vector2 keyBoard = _input.Player.Move.ReadValue<Vector2>();
        //Debug.Log(mouseMove);
        //Debug.Log(keyBoard);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    // FixedUpdate is called once per physics update
    private void FixedUpdate()
    {
        
    }

    private void registerMovement()
    {

    }


    private void CameraRotation()
    {


        Vector2 playerLookDif = _input.Player.Look.ReadValue<Vector2>();
        // if there is an input
        if (playerLookDif.sqrMagnitude >= MouseMoveThreshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            //The mouse delta position already heeds for frame rate
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            

            _cinemachineTargetPitch -= playerLookDif.y * MouseSensitivity * deltaTimeMultiplier;
            _rotationVelocity = playerLookDif.x * MouseSensitivity * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            //Debug.Log("target Pitch: " + _cinemachineTargetPitch);
            //Debug.Log(_rotationVelocity);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
            
        }
    }


    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
