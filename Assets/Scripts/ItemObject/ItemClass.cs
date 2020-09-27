using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemClass {


    public int GlobalID;
    [HideInInspector] public int CategoryID;
    [HideInInspector] public CategoryName CategoryName;
    [HideInInspector] public int TypeID;
    public string TypeName;
    [Range(1, 100)] public int Level;
    public Quality Quality;
    [HideInInspector] public IntVector2 Size;
    [HideInInspector] public Sprite Icon;
    [HideInInspector] public string SerialID;
    public List<StatBonus> StatBonuses = new List<StatBonus>();

    public static void SetItemValues(ItemClass item)
    {
        ItemDatabase.Instance.PassItemData(ref item);
    }


    public ItemClass(ItemClass passedItem)//create new item by copying passedITem properties
    {
        GlobalID = passedItem.GlobalID;
        CategoryName = ItemDatabase.Instance.dbList[GlobalID].CategoryName;
        Level = passedItem.Level;
        Quality = passedItem.Quality;
        foreach (StatBonus stat in passedItem.StatBonuses)
            StatBonuses.Add(stat);
        SetItemValues(this);
    }

    public ItemClass(int id, int level, int quality, List<StatBonus> stats)
    {
        GlobalID = id;
        CategoryName = ItemDatabase.Instance.dbList[GlobalID].CategoryName;
        Level = level;
        Quality = ItemDatabase.Instance.Qualities[quality];
        StatBonuses = stats.ToList();
        SetItemValues(this);
    }

    public ItemClass()
    {

    }
}