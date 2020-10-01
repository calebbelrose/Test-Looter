using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CombatController))]
public class Enemy : Interactable
{
    public CombatController CombatController;
    int Armour, Damage;
    public NavMeshAgent enemyAgent;
    public bool chase = false;

    private void Update()
    {
        if (chase)
        {
            enemyAgent.destination = playerAgent.transform.position;
            transform.LookAt(playerAgent.transform);
            if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
            {
                CombatController.Animator.SetBool("Walking", false);
                CombatController.PerformAttack();
            }
            else
            {
                CombatController.Animator.SetBool("Walking", true);
            }
        }

        CheckDistance();
    }

    public override void MoveToInteraction(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 6;
        playerAgent.destination = transform.position;
        hasInteracted = false;
    }

    public override void Interact()
    {
        playerAgent.GetComponent<CombatController>().PerformAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerAgent = other.GetComponent<NavMeshAgent>();
            chase = true;
            enemyAgent.stoppingDistance = 6;
        }
    }
}
