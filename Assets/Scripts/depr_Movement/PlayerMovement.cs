using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform orientation;

    [Header("Ground detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    /// <summary>
    /// Main Objects
    /// </summary>
    private InputMaster _input;


    /// <summary>
    /// Public objects and variables
    /// </summary>
    [Header("Movement")]
    [SerializeField] float walkSpeed = 60f;
    [SerializeField] float sprintSpeed = 100f;
    [SerializeField] float acceleration = 10f;

    [Space(10)]
    [Header("Slopes")]


    [Header("Jumping")]
    public int totAirJumps = 2;
    public float jumpForce = 10f;

    /// <summary>
    /// private objects and variables
    /// </summary>

    private Rigidbody rb;
    private float horizontalMovement;
    private float verticallMovement;

    // Move direction
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    private Vector2 userMoveInput;
    private bool isGroudet;

    //Slopes
    RaycastHit slopeHit;
    bool onSlope;

    private bool OnSlope()
    {
        // check if hit surface is not even
        Physics.Raycast(groundCheck.transform.position, Vector3.down, out slopeHit, groudCheckRadius);
        // slope detectet, if the normal of the surace is not facing upwards
        if(slopeHit.normal != Vector3.up)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //move modifiers
    private float playerSpeed = 60f;
    private float airSpeedModifyer = 0.2f;

//Jumping Modifiers
private bool doJump;
    private int airJumpsLeft;
    private float groudDrag = 5f;
    private float airDrag = 0.6f;
    private float groudCheckRadius = 0.1f;


    // called when skript is loaded
    private void Awake()
    {
        _input = new InputMaster();
        _input.Player.Jump.performed += Jump;
    }


    //called when the object becomes enabled and active
    private void OnEnable()
    {
        _input.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //get consistent hight in case we want to do shananigans with it
        // float playerHight = transform.lossyScale.y;
        isGroudet = IsGroudet();
        playerSpeed = SetSprintSpeed(playerSpeed) ;
        moveDirection = MyInput(_input.Player.Move);
        DragControl();

        //reset Jump counter when on ground
        if (isGroudet) airJumpsLeft = totAirJumps;

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

    }

    private bool IsGroudet()
    {
        return Physics.CheckSphere(groundCheck.position, groudCheckRadius, groundMask);
    }



    private float SetSprintSpeed(float _playerSpeed)
    {
        // is 
        if (_input.Player.Sprint.IsPressed())
        {
            _playerSpeed = Mathf.Lerp(_playerSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            _playerSpeed = Mathf.Lerp(_playerSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
         return (_playerSpeed);
    }


    //called every fixed update
    private void FixedUpdate()
    {
        MovePlayer();

    }


    private void Jump(InputAction.CallbackContext context)
    {
        // resetz horizontal velocity if ground check returned true
        // in case rb does not actually touch ground (has still y-velocity)

        if (isGroudet)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        else
        // airborne lower airjump counter by one
        {
            airJumpsLeft -= 1;
        }

        if (airJumpsLeft >= 0 || isGroudet)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }



    //---------------------------USER FUNCTION -------------------------
    /// <summary>
    /// Move player Object
    /// </summary>
    private void MovePlayer()
    {
        if (isGroudet && !onSlope)
        {
            rb.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Acceleration);   
        }
        else if (isGroudet && onSlope)
        {
            rb.AddForce(slopeMoveDirection.normalized * playerSpeed, ForceMode.Acceleration);
        }
        else if (!isGroudet)
        {
            rb.AddForce(moveDirection.normalized * playerSpeed * airSpeedModifyer, ForceMode.Acceleration);
        }



    }


    /// <summary>
    /// Manage user Drag 
    /// </summary>
    private void DragControl()
    {
        if (isGroudet)
        {
            rb.drag = groudDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    /// <summary>
    /// get user "Move" Input and turn it into Vector3 
    /// <returns>Vector 3 move direction</returns>
    Vector3 MyInput(InputAction move)
    {
        // read 2D user Input (x,y)
        userMoveInput = move.ReadValue<Vector2>();

        //get values for x and y
        horizontalMovement = userMoveInput.x;
        verticallMovement = userMoveInput.y;

        // convert to 3D Vector (x,y,z)
        return (orientation.transform.forward * verticallMovement + orientation.right * horizontalMovement);
    }

}
