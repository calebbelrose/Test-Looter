using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<Stat> Stats = new List<Stat>();

    private static CharacterStats instance = null;

    public static int MaxLevel = 100;
    public int Level = 1;
    public int Health = 100;
    public List<EquipSlot> EquipSlots = new List<EquipSlot>();

    private void Start()
    {
        foreach (StatName stat in System.Enum.GetValues(typeof(StatName)))
            Stats.Add(new Stat(stat, 0));
    }

    public void Equip(ItemScript item, EquipSlot equipSlot)
    {
        if (item.item.CategoryName == equipSlot.CategoryName)
        {
            item.transform.SetParent(equipSlot.transform);
            item.transform.localPosition = Vector3.zero;
            item.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            if (equipSlot.Empty)
            {
                equipSlot.Item = item;
                ItemScript.ResetSelectedItem();
            }
            else
            {
                ItemScript tempItem = equipSlot.Item;
                RemoveStatBonus(equipSlot.Item.item.StatBonuses);
                equipSlot.Item = item;
                ItemScript.SetSelectedItem(tempItem);
            }

            AddStatBonus(equipSlot.Item.item.StatBonuses);
            equipSlot.Empty = false;
            equipSlot.Image.color = equipSlot.Item.item.Quality.Colour;
        }
    }

    public void Unequip(EquipSlot equipSlot)
    {
        RemoveStatBonus(equipSlot.Item.item.StatBonuses);
        ItemScript.SetSelectedItem(equipSlot.Item);
        equipSlot.Empty = true;
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

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.transform);
    }
}