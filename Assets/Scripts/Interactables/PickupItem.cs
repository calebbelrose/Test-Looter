using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    //Picks up item
    public override void Interact()
    {
        Debug.Log("Pickup");
    }
}
