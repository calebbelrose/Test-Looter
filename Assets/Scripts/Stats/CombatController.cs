﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatController : MonoBehaviour
{
    public float MagicFind { get; private set; } = 1f;
    public int Level { get; private set; } = 1;
    public Animator Animator { get { return animator; } }


    [SerializeField] protected List<EquipSlot> EquipSlots = new List<EquipSlot>();

    [SerializeField] private Animator animator;
    [SerializeField] private Weapon Weapon;
    [SerializeField] private List<Stat> Stats = new List<Stat>();
    [SerializeField] private List<CommonLootDrop> CommonLootDrops = new List<CommonLootDrop>();
    [SerializeField] private Transform WeaponLocation;

    protected float CurrentHealth = 100, MaxHealth = 100;
    protected float CurrentResource = 100, MaxResource = 100;
    protected float experience = 0, experienceToNextLevel = 100;

    public static int MaxLevel { get; } = 100;

    //Adds stat bonus
    public void AddStatBonus(List<StatBonus> statBonuses)
    {
        foreach (StatBonus statBonus in statBonuses)
            Stats.Find(x => x.StatName == statBonus.StatName).AddStatBonus(statBonus);
    }

    //Removes stat bonus
    public void RemoveStatBonus(List<StatBonus> statBonuses)
    {
        foreach (StatBonus statBonus in statBonuses)
            Stats.Find(x => x.StatName == statBonus.StatName).RemoveStatBonus(statBonus);
    }

    //Performs attack
    public void PerformAttack()
    {
        Animator.SetTrigger("Attacking");
    }

    //Takes damage
    public virtual void TakeDamage(float amount, CombatController attacker)
    {
        if (amount > 0)
        {
            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                int weightSum = 0;
                int index = 0;
                int roll;

                foreach (LootDrop drop in CommonLootDrops)
                    weightSum += drop.Weight;

                roll = Random.Range(0, weightSum);

                while(index < CommonLootDrops.Count && roll <= CommonLootDrops[index].Weight)
                        roll -= CommonLootDrops[index].Weight;

                //ItemDatabase.Instance.SpawnLoot(transform.position, CommonLootDrops[index].ItemID, Random.Range(CommonLootDrops[index].MinAmount, CommonLootDrops[index].MaxAmount));
                ItemDatabase.Instance.SpawnLoot(transform.position, attacker);
                attacker.AwardExperience(Level * MaxHealth);
                Destroy(gameObject);
            }
        }
    }

    //Uses resource
    public virtual void UseResource(float amount)
    {
        CurrentResource -= amount;
    }

    //Awards experience
    protected virtual void AwardExperience(float amount)
    {
        experience += amount;
        while (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.25);
            Level++;
        }
    }

    //Returns ordered stats
    public List<Stat> OrderedStats()
    {
        return Stats.OrderBy(x => UnityEngine.Random.value).Take(UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(StatName)).Length)).ToList();
    }

    //Returns damage
    public float Damage
    {
        get
        {
            return Weapon.Damage * (Crit ? CritMultiplier * StrengthMultiplier : StrengthMultiplier);
        }
    }

    //Returns crit chance
    public bool Crit
    {
        get
        {
            float Luck = Stats.Find(x => x.StatName.ToString() == "Luck").FinalValue;

            return Random.Range(0, Luck + 10000f) <= Luck;
        }
    }

    //Returns crit damage
    public float CritMultiplier
    {
        get
        {
            return 1 + Stats.Find(x => x.StatName.ToString() == "Dexterity").FinalValue / 1000f;
        }
    }

    //Returns strength multiplier
    public float StrengthMultiplier
    {
        get
        {
            return 1 + Stats.Find(x => x.StatName.ToString() == "Strength").FinalValue / 1000f;
        }
    }
}