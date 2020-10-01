using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using UnityEditor.Animations;

public class WorldInteraction : MonoBehaviour
{
    public NavMeshAgent playerAgent;
    public Animator Animator;

    private void Update()
    {
        if(playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            Animator.SetBool("Walking", false);
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            GetInteractions();
    }

    void GetInteractions()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;

        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject;

            Animator.SetBool("Walking", true);

            if (interactedObject.tag == "Interactable")
                interactedObject.GetComponent<Interactable>().MoveToInteraction(playerAgent);
            else
            {
                playerAgent.destination = interactionInfo.point;
                playerAgent.stoppingDistance = 1.5f;
            }
        }

        playerAgent.transform.LookAt(playerAgent.destination);
    }
}
