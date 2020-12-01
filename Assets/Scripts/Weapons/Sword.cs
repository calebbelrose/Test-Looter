using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    //Performs sword attack
    public void PerformAttack()
    {
        Debug.Log("Sword attack");
    }
}