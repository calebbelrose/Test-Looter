using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Interactable
{
    public string[] Dialogue;
    public string Name;

    public override void Interact()
    {
        DialogueSystem.Instance.AddNewDialogue(Name, Dialogue);
        Debug.Log("NPC");
    }
}
