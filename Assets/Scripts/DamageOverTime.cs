using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public CombatController CombatController;
    public float Damage;

    private float secondsBetweenTicks = 1f;
    List<CombatController> enemies = new List<CombatController>();

    //Adds enemy to list of characters taking damage
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            CombatController otherCombatController = other.GetComponent<CombatController>();

            if (otherCombatController != null && CombatController.tag != otherCombatController.tag)
            {
                enemies.Add(otherCombatController);
                StartCoroutine(DealDamage(otherCombatController));
            }
        }
    }

    //Removes enemy from list of characters taking damage
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            CombatController otherCombatController = other.GetComponent<CombatController>();

            if (otherCombatController != null && CombatController.tag != otherCombatController.tag)
                enemies.Remove(otherCombatController);
        }
    }

    //Deals damage to enemy
    private IEnumerator DealDamage(CombatController otherCombatController)
    {
        while (enemies.Contains(otherCombatController))
        {
            otherCombatController.TakeDamage(Damage, CombatController);
            yield return new WaitForSeconds(secondsBetweenTicks);
        }
    }
}
