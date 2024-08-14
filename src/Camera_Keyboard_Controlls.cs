using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera_Keyboard_Controlls : MonoBehaviour
{
    public float angle = 0.2f;
    public GameObject centre_object = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 centre = centre_object.transform.position;
        if (Input.GetKey("i"))
        {
            transform.RotateAround(centre, Vector3.right, angle * Time.deltaTime);
        }
        if (Input.GetKey("k"))
        {
            transform.RotateAround(centre, Vector3.left, angle * Time.deltaTime);
        }
        if (Input.GetKey("l"))
        {
            transform.RotateAround(centre, Vector3.up, angle * Time.deltaTime);
        }
        if (Input.GetKey("j"))
        {
            transform.RotateAround(centre, Vector3.down, angle * Time.deltaTime);
        }
    }
}
