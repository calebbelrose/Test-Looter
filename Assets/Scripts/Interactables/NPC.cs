using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Interactable
{
    [SerializeField] private string[] Dialogue;
    [SerializeField] private string Name;

    //Displays NPC's dialogue
    public override void Interact()
    {
        DialogueSystem.Instance.AddNewDialogue(Name, Dialogue);
        Debug.Log("NPC");
    }
}
