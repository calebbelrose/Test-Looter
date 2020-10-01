using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatController : MonoBehaviour
{
    public float CurrentHealth = 100, MaxHealth = 100;
    public float CurrentResource = 100, MaxResource = 100;
    public List<Stat> Stats = new List<Stat>();
    public Animator Animator;
    public Weapon Weapon;
    public static int MaxLevel = 100;
    public int Level = 1;
    public List<EquipSlot> EquipSlots = new List<EquipSlot>();
    public Transform WeaponLocation;

    protected float experience = 0, experienceToNextLevel = 100;

    private void Start()
    {
        foreach (StatName stat in System.Enum.GetValues(typeof(StatName)))
            Stats.Add(new Stat(stat, 0));
    }

    public void AddStatBonus(List<StatBonus> statBonuses)
    {
        foreach (StatBonus statBonus in statBonuses)
            Stats.Find(x => x.StatName == statBonus.StatName).AddStatBonus(statBonus);
    }

    public void RemoveStatBonus(List<StatBonus> statBonuses)
    {
        foreach (StatBonus statBonus in statBonuses)
            Stats.Find(x => x.StatName == statBonus.StatName).RemoveStatBonus(statBonus);
    }

    public void PerformAttack()
    {
        Animator.SetTrigger("Attacking");
    }

    public virtual void TakeDamage(float amount, CombatController attacker)
    {
        if (amount > 0)
        {
            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
                attacker.AwardExperience(Level * MaxHealth);
            }
        }
    }

    public virtual void UseResource(float amount)
    {
        CurrentResource -= amount;
    }

    protected virtual void AwardExperience(float amount)
    {
        experience += amount;
        if (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.25);
        }
    }

    public float Damage
    {
        get
        {
            return Weapon.Damage * (Crit ? CritMultiplier * StrengthMultiplier : StrengthMultiplier);
        }
    }

    public bool Crit
    {
        get
        {
            float Luck = Stats.Find(x => x.StatName.ToString() == "Luck").FinalValue;
            return Random.Range(0, Luck + 10000f) <= Luck;
        }
    }

    public float CritMultiplier
    {
        get
        {
            return 1 + Stats.Find(x => x.StatName.ToString() == "Dexterity").FinalValue / 1000f;
        }
    }

    public float StrengthMultiplier
    {
        get
        {
            return 1 + Stats.Find(x => x.StatName.ToString() == "Strength").FinalValue / 1000f;
        }
    }
}
