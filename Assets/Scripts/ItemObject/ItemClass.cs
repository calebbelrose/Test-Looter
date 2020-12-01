using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemClass
{
    public List<StatBonus> StatBonuses = new List<StatBonus>();
    public int GlobalID { get; private set; }
    public int CategoryID { get; private set; }
    public CategoryName CategoryName { get; private set; }
    public string TypeName { get; private set; }
    public IntVector2 Size { get; private set; }
    public Sprite Icon { get; private set; }
    public Quality Quality { get; private set; }
    public int Level { get { return level; } }
    public string StatString
    {
        get
        {
            string statBonuses = "";

            foreach (StatBonus stat in StatBonuses)
                statBonuses += "," + (int)stat.StatName + "," + stat.BonusValue;

            return statBonuses;
        }
    }

    [Range(1, 100)] private int level;

    public ItemClass(int id, int level, int quality, List<StatBonus> stats)
    {
        ItemData itemData = ItemDatabase.Instance.ItemData(id);

        GlobalID = id;
        this.level = level;
        Quality = ItemDatabase.Instance.Quality(quality);
        StatBonuses = stats.ToList();
        CategoryID = itemData.CategoryID;
        CategoryName = itemData.CategoryName;
        TypeName = itemData.TypeName;
        Size = itemData.Size;
        Icon = itemData.Icon;
    }

    public ItemClass(int id)
    {
        ItemData itemData = ItemDatabase.Instance.ItemData(id);

        GlobalID = id;
        level = 1;
        Quality = ItemDatabase.Instance.Quality(1);
        CategoryID = itemData.CategoryID;
        CategoryName = itemData.CategoryName;
        TypeName = itemData.TypeName;
        Size = itemData.Size;
        Icon = itemData.Icon;
    }
}