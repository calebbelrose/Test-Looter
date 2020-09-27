using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CategoryName CategoryName;
    public Image Image;
    public ItemScript Item;
    public CharacterStats characterStats;

    public bool Empty = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemScript.selectedItem != null)
        {
            if (ItemScript.selectedItem.item.CategoryName == CategoryName)
                Image.color = Color.green;
            else
                Image.color = Color.red;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Empty)
        {
            characterStats.Unequip(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (ItemScript.selectedItem != null)
            {
                characterStats.Equip(ItemScript.selectedItem, this);
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Empty)
            Image.color = Color.white;
        else
            Image.color = Item.item.Quality.Colour;
    }
}