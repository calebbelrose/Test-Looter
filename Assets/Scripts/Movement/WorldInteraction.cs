using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class WorldInteraction : MonoBehaviour
{
    public NavMeshAgent playerAgent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            GetInteractions();
    }

    void GetInteractions()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject;
            if (interactedObject.tag == "Interactable")
            {
                interactedObject.GetComponent<Interactable>().MoveToInteraction(playerAgent);
            }
            else
            {
                playerAgent.destination = interactionInfo.point;
                playerAgent.stoppingDistance = 0;
            }
        }
    }
}
