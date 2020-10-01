using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : CombatController
{
    public Image HealthBar;
    public Image HealthBarChild;
    public Image ResourceBarChild;
    public Image ExperienceBar;
    public ResourceLevel ResourceBar;
    public RectTransform HealthBar2;

    public override void TakeDamage(float amount, CombatController attacker)
    {
        CurrentHealth -= amount;
    }

    protected override void AwardExperience(float amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.25);
        }

        ExperienceBar.fillAmount = experience / experienceToNextLevel;
    }

    private void Update()
    {
        float Health = CurrentHealth / MaxHealth;
        HealthBar.fillAmount = Health;
        HealthBar2.sizeDelta = new Vector2(HealthBar2.sizeDelta.x, HealthBar2.sizeDelta.x * Health);
        float Resource = CurrentResource / MaxResource;
        ResourceBar.SetLevel(Resource);
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
}