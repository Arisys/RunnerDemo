using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderOrientation : MonoBehaviour
{

    [SerializeField] Transform cam;

    Transform itemHolderTransform;

    // Start is called before the first frame update
    void Start()
    {
        itemHolderTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        itemHolderTransform.rotation = cam.rotation;
    }
}
