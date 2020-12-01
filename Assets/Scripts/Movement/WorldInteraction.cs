using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using UnityEditor.Animations;

public class WorldInteraction : MonoBehaviour
{
    [SerializeField] private NavMeshAgent PlayerAgent;
    [SerializeField] private Animator Animator;
    [SerializeField] private LayerMask LayerMask;

    //Moves player agent to destination
    private void Update()
    {
        if (PlayerAgent.remainingDistance <= PlayerAgent.stoppingDistance)
        {
            Vector3 dir = PlayerAgent.destination - transform.position;
            Quaternion rot;

            dir.y = 0;
            rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f * Time.deltaTime);
            Animator.SetBool("Walking", false);
        }

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            GetInteractions();
    }

    //Tells player agent where to move to
    void GetInteractions()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;

        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity, LayerMask))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject;

            Animator.SetBool("Walking", true);

            if (interactedObject.tag == "Interactable")
                interactedObject.GetComponent<Interactable>().MoveToInteraction(PlayerAgent);
            else
            {
                PlayerAgent.destination = interactionInfo.point;
                PlayerAgent.stoppingDistance = 1.5f;
            }
        }
    }
}
