using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private CharacterStats characterStats;

    //Spawns loot from chest
    public override void Interact()
    {
        characterStats = playerAgent.gameObject.GetComponent<CharacterStats>();

        for (int i = 0; i < Random.Range(0, 10); i++)
            ItemDatabase.Instance.SpawnLoot(transform.position, characterStats);

        Destroy(gameObject);
    }
}
