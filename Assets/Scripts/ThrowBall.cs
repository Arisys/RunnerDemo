using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowBall : MonoBehaviour
{

    private bool readyToTrow;
    private float throwCooldown = 1f;
    private float reloadCooldown = 2f;

    private int totalThrows = 10;
    private int throwsLeft = 50;
    private Rigidbody projectileRb;
    private Vector3 throwDirection;
    private InputMaster _input;

    [SerializeField] GameObject objectToThrow;
    [SerializeField] Transform throwingPoint;
    [SerializeField] float throwForce;
 

    private void Awake()
    {
        _input = new InputMaster();
    }

    private void Start()
    {
        _input.Player.Shoot_Throw.performed += Throw;
        readyToTrow = true;
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }


    private void Throw(InputAction.CallbackContext obj)
    {
        //return if not ready to trow
        if (!readyToTrow || throwsLeft <= 0 )
        {
            Debug.Log("Trow on cooldown");
            return;
        }

        readyToTrow = false;
        throwsLeft--;
        Debug.Log("throwsLeft"+ throwsLeft);

        //instanciate projectile
        GameObject projectile = Instantiate(objectToThrow, throwingPoint.position, throwingPoint.rotation);

        projectileRb = projectile.GetComponent<Rigidbody>();

        //simply throw the Ball foreward
        throwDirection = throwingPoint.forward + throwingPoint.up * 0.3f;

        projectileRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ReloadThrows()
    {
        if (throwsLeft < totalThrows)
        {
            throwsLeft += 1;
        }
    }

    private void ResetThrow()
    {
        readyToTrow = true;
    }
}
