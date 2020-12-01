using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageForTicks : MonoBehaviour
{
    public CombatController CombatController;
    public float Damage;

    private List<TickingCharacter> tickingCharacters = new List<TickingCharacter>();
    private int ticks = 4;
    private float secondsBetweenTicks = 1f;

    //Adds or refreshes on enemy
    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
        {
            CombatController otherCombatController = other.GetComponent<CombatController>();

            if (otherCombatController != null && CombatController.tag != otherCombatController.tag)
            {
                int index = tickingCharacters.FindIndex(x => x.Enemy == otherCombatController);

                if (index >= 0)
                    tickingCharacters[index].Ticks = ticks;
                else
                {
                    tickingCharacters.Add(new TickingCharacter(otherCombatController, ticks));
                    StartCoroutine(DealDamage(otherCombatController, tickingCharacters.Count - 1));
                }
            }
        }
    }

    //Deals tick damage to enemy
    private IEnumerator DealDamage(CombatController otherCombatController, int index)
    {
        while (tickingCharacters[index].Ticks > 0)
        {
            otherCombatController.TakeDamage(Damage, CombatController);
            tickingCharacters[index].Ticks--;
            yield return new WaitForSeconds(secondsBetweenTicks);
        }
    }
}