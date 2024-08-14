using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Valve.VR;

public class PickUp : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean inputBool;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    private bool isTriggering = false;
    private List<Collider> interactables = new List<Collider>();
    private List<Collider> interatingables = new List<Collider>();
    private List<Collider> interatingablesDel = new List<Collider>();
    public Transform ship;

    void Start()
    {
        inputBool.AddOnStateDownListener(TriggerDown, handType);
        inputBool.AddOnStateUpListener(TriggerUp, handType);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isTriggering = false;
        foreach (Collider col in interatingables)
        {
            if (col.transform.parent == transform)
            {
                col.transform.parent = ship;
               
            }
            interatingablesDel.Add(col);
        }
        foreach (Collider col in interatingablesDel) {
            interatingables.Remove(col);
        }
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isTriggering = true;
        foreach (Collider col in interactables) { 
            col.transform.parent = transform;
            interatingables.Add(col);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Interactable" && !interactables.Contains(other)) {
            interactables.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable" && interactables.Contains(other))
        {
            interactables.Remove(other);
        }
    }
}
