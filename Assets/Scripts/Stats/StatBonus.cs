using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBonus : IEquatable<StatBonus>
{
    public int BonusValue;
    public StatName StatName;

    public StatBonus(StatName name, int bonusValue)
    {
        StatName = name;
        BonusValue = bonusValue;
    }
    public bool Equals(StatBonus other)
    {
        return StatName == other.StatName;
    }
}
