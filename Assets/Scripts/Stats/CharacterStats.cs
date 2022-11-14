using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : CombatController
{
    [SerializeField] private Image ExperienceBar;
    [SerializeField] private ResourceLevel HealthBar, ManaBar;
    [SerializeField] private Transform ListParent;

    private void Start()
    {
        LoadEquipment();
    }

    //Takes damage
    public override void TakeDamage(float amount, CombatController attacker)
    {
        CurrentHealth -= amount;
        HealthBar.SetLevel(CurrentHealth / MaxHealth);
    }

    //Uses Resource
    public override void UseResource(float amount)
    {
        CurrentResource -= amount;
        ManaBar.SetLevel(CurrentResource / MaxResource);
    }

    //Awards experience
    protected override void AwardExperience(float amount)
    {
        experience += amount;

        while (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.25);
        }

        ExperienceBar.fillAmount = experience / experienceToNextLevel;
    }

    //Equips item
    public void Equip(ItemScript itemScript)
    {
        EquipSlot equipSlot = EquipSlots.Find(x => x.CategoryName == itemScript.item.CategoryName);
        EquipInSlot(itemScript, equipSlot);
    }
    
    //Equips item in slot
    public void EquipInSlot(ItemScript itemScript, EquipSlot equipSlot)
    {
        if (itemScript.item.CategoryName == equipSlot.CategoryName)
        {
            itemScript.transform.SetParent(equipSlot.transform);
            itemScript.transform.localPosition = Vector3.zero;
            itemScript.CanvasGroup.alpha = 1f;

            if (equipSlot.Empty)
                ItemScript.ResetSelectedItem();
            else
            {
                RemoveStatBonus(equipSlot.Item.item.StatBonuses);
                ItemScript.SetSelectedItem(equipSlot.Item);
            }

            equipSlot.Item = itemScript;
            AddStatBonus(itemScript.item.StatBonuses);
            equipSlot.Empty = false;
            equipSlot.Image.color = itemScript.item.Quality.Colour;
            equipSlot.EquipObject.SetActive(true);
        }
    }

    //Saves equipment to its slot
    public void SaveEquipment(ItemScript itemScript)
    {
        StreamWriter sw;

        sw = File.AppendText("./Assets/Scripts/CreateItem/SavedEquipment.csv");
        sw.WriteLine(itemScript.item.CategoryName + "," + itemScript.item.GlobalID + "," + itemScript.item.Level + "," + ItemDatabase.Instance.Quality(itemScript.item.Quality) + itemScript.item.StatString);
        sw.Close();
    }

    //Loads equipment to its slot
    private void LoadEquipment()
    {
        string[] lines = File.ReadAllLines("./Assets/Scripts/CreateItem/SavedEquipment.csv");

        for (int i = 0; i < lines.Length; i++)
        {
            List<StatBonus> statBonuses = new List<StatBonus>();
            string[] data = lines[i].Split(',');
            ItemClass item;
            ItemScript itemScript = ItemDatabase.Instance.ItemEquipPool.GetItemScript();

            for (int j = 4; j < data.Length; j += 2)
                statBonuses.Add(new StatBonus((StatName)int.Parse(data[j]), int.Parse(data[j + 1])));

            item = new ItemClass(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), statBonuses);
            itemScript.SetItemObject(item);
            Equip(itemScript);
            itemScript.Rect.localScale = Vector3.one;
        }
    }

    //Unequips item
    public void Unequip(EquipSlot equipSlot)
    {
        RemoveStatBonus(equipSlot.Item.item.StatBonuses);
        ItemScript.SetSelectedItem(equipSlot.Item);
        equipSlot.Empty = true;
        File.WriteAllLines("./Assets/Scripts/CreateItem/SavedEquipment.csv", File.ReadLines("./Assets/Scripts/CreateItem/SavedEquipment.csv").Where(line => line.Split(',')[0] != equipSlot.CategoryName.ToString()).ToList());
    }

    //Lists learned professions
    public void ListLearnedProfessions(string profession)
    {
        (Activator.CreateInstance(Type.GetType(profession)) as Profession).ListItems(ListParent);
    }

    //Lists unlearned professions
    public void ListUnlearnedProfessions(string profession)
    {

    }
}