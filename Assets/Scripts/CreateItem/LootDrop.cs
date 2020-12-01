using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Loot drop
[Serializable]
public class LootDrop
{
    [SerializeField] public int ItemID { get { return itemID; } }
    [SerializeField] public int Weight { get { return weight; } }

    [SerializeField] private int itemID;
    [SerializeField] private int weight;

    public LootDrop(int id, int weight)
    {
        itemID = id;
        this.weight = weight;
    }
}

//Common loot drop
public class CommonLootDrop : LootDrop
{
    public int MinAmount { get; private set; }
    public int MaxAmount { get; private set; }

    public CommonLootDrop(int id, int weight, int minAmount, int maxAmount) : base(id, weight)
    {
        MinAmount = minAmount;
        MaxAmount = maxAmount;
    }
}