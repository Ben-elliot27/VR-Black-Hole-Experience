using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ThrustLever : MonoBehaviour
{
    public CircularDrive circ;
    public Animation anim;
    public SteamVR_Action_Boolean inputBool;
    public SteamVR_Input_Sources handType;
    // Start is called before the first frame update
    void Start()
    {
        //inputBool.AddOnStateDownListener(TriggerDown, handType);
    }

  
    // Update is called once per frame
    void Update()
    {

        anim["RocketMove"].speed = circ.outAngle / circ.maxAngle;
    }

    void onKeyPress()
    {

    }


    //public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
   // {
   //     anim.Play();
   // }
}