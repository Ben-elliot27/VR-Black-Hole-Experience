using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotationScript : MonoBehaviour
{

    public float rotate_angle = 0.5f;
    public GameObject centre_object = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Check if the object to rotate and angle are assigned
        if (centre_object != null)
        {
            // Get the center of the object to rotate
            Vector3 center = centre_object.transform.position;

            // Rotate the object around the center by the given angle
            transform.RotateAround(center, Vector3.up, rotate_angle);
        }
        else
        {
            Debug.LogWarning("Object to rotate is not assigned.");
        }
    }
}
