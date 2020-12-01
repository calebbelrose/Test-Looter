using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    protected NavMeshAgent playerAgent;
    protected bool hasInteracted;

    //Sets the destination of the agent to the location of this object
    public virtual void MoveToInteraction(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 3;
        playerAgent.destination = transform.position;
        hasInteracted = false;
    }

    void Update()
    {
        CheckDistance();
    }

    //Interacts when the agent reaches the destination
    protected void CheckDistance()
    {
        if (!hasInteracted && playerAgent != null && !playerAgent.pathPending)
        {
            if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }    

    //Interaction action
    public virtual void Interact()
    {
        Debug.Log("Base class");
    }
}
