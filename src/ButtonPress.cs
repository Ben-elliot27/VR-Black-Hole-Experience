using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public RotationLate script;
    public Transform rocket;
    private Vector3 startPos;
    private bool alreadyEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        startPos = rocket.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if (alreadyEnabled)
        {
            rocket.position = startPos;
            script.rotating = true;
            alreadyEnabled = false;
        }
        else
        {
            script.rotating = false;
            alreadyEnabled=true;
        }
        
    }
}
