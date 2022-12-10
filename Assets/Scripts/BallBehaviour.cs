using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] Transform cameraReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Invoke(nameof(DestroyBall), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroyBall()
    {
        this.transform.localScale *=3;

        Invoke(nameof(DestroyGameobject), 1f);
        
    }

    private void DestroyGameobject()
    {
        Destroy(this.gameObject);
    }
}



