using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private bool hasInteracted;

    public virtual void MoveToInteraction(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 3;
        playerAgent.destination = transform.position;
        hasInteracted = false;
    }

    void Update()
    {
        if(!hasInteracted && playerAgent!=null && !playerAgent.pathPending)
        {
            if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Base class");
    }
}
