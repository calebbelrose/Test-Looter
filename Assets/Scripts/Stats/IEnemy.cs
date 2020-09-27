using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int CurrentHealth = 100, MaxHealth, Power, Toughness;
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }
    public void PerformAttack()
    {

    }
}
