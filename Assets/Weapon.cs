using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public CombatController CombatController;
    public float MinDamage, MaxDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            CombatController otherCombatController = other.GetComponent<CombatController>();
            if (otherCombatController != null && CombatController.tag != otherCombatController.tag)
                otherCombatController.TakeDamage(CombatController.Damage, CombatController);
        }
    }

    public float Damage
    {
        get
        {
            return Random.Range(MinDamage, MaxDamage);
        }
    }
}