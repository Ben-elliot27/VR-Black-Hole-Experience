using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpin : MonoBehaviour
{
    public List<Transform> bunghole;
    public float maxDist;
    public float speed;
    private float speed2;
    private Transform closest;

    void FixedUpdate()
    {
        float smallestD = 100000f;
        foreach (Transform t in bunghole) {

            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < smallestD) {
                smallestD = dist;
                closest = t;
            }
        }
        if (closest.tag == "Wormhole")
        {
            // Spin backwards -- in wormhole
            transform.Rotate(Vector3.forward * -speed);

        }
        else
        {
            // Spin normally
            if (smallestD < maxDist)
            {
                speed2 = speed * (smallestD / maxDist);
            }
            else
            {
                speed2 = speed;
            }
            transform.Rotate(Vector3.forward * speed2);
        }
    }
}