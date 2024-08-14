using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEarth : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Earth;
    public bool onEarth = true;
    void Start()
    {
        transform.parent = Earth;
    }

    // Update is called once per frame
    void Update()
    {
        if (onEarth)
        {
            transform.parent = Earth;
        }
        else
        {
            transform.parent = null;
        }
    }
}
