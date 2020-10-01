using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public StatName StatName;

    private List<StatBonus> StatBonuses = new List<StatBonus>();
    private List<StatMultiplier> StatMultipliers = new List<StatMultiplier>();
    [SerializeField]private int baseValue;

    public Stat(StatName name, int _baseValue)
    {
        StatName = name;
        baseValue = _baseValue;
    }

    public void ChangeStat(int value)
    {
        baseValue = value;
    }

    public void AddStatBonus(StatBonus statBonus)
    {
        StatBonuses.Add(statBonus);
    }

    public void RemoveStatBonus(StatBonus statBonus)
    {
        StatBonuses.Remove(StatBonuses.Find(x => x.BonusValue == statBonus.BonusValue));
    }

    public int FinalValue
    {
        get
        {
            int finalValue = baseValue;
            StatBonuses.ForEach(x => finalValue += x.BonusValue);
            return finalValue;
        }
    }
}