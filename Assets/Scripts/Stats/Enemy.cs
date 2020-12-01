
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CombatController))]
public class Enemy : Interactable
{
    [SerializeField] private CombatController CombatController;
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private LayerMask playerLayer;

    private bool chase = false;
    private Collider[] players;


    //Chases player in range
    private void Update()
    {
        if (chase)
        {
            enemyAgent.destination = playerAgent.transform.position;

            if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
            {
                Vector3 dir = enemyAgent.destination - transform.position;
                Quaternion rot;

                dir.y = 0;
                rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f * Time.deltaTime);
                CombatController.Animator.SetBool("Walking", false);
                CombatController.PerformAttack();
            }
            else
                CombatController.Animator.SetBool("Walking", true);
        }

        CheckDistance();
    }

    //Chases the closest player that is able to be seen and in rage
    private void FixedUpdate()
    {
        players = Physics.OverlapSphere(transform.position, 10f, playerLayer);

        int bestIndex = -1;
        float bestDistance = float.MaxValue;

        for (int i = 0; i < players.Length; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, (players[i].transform.position - transform.position), out hit) && hit.transform.CompareTag("Player") && hit.distance < bestDistance)
            {
                bestDistance = hit.distance;
                bestIndex = i;
            }
        }

        if (bestIndex != -1)
        {
            playerAgent = players[bestIndex].GetComponent<NavMeshAgent>();
            chase = true;
            enemyAgent.stoppingDistance = 6;
        }
    }

    //Moves player to enemy
    public override void MoveToInteraction(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 6;
        playerAgent.destination = transform.position;
        hasInteracted = false;
    }

    //Causes player to attack enemy
    public override void Interact()
    {
        playerAgent.GetComponent<CombatController>().PerformAttack();
    }
}
