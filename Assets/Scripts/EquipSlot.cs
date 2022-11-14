using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool Empty = true;
    public ItemScript Item;

    public Image Image { get { return image; } }
    public GameObject EquipObject { get { return equipObject; } }

    public CategoryName CategoryName { get { return categoryName; } }

    [SerializeField] private CharacterStats CharacterStats;
    [SerializeField] private CategoryName categoryName;
    [SerializeField] private Image image;
    [SerializeField] private GameObject equipObject;

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
            CharacterStats.Unequip(this);
        else if (eventData.button == PointerEventData.InputButton.Left && ItemScript.selectedItem != null)
        {
            CharacterStats.SaveEquipment(ItemScript.selectedItem);
            CharacterStats.EquipInSlot(ItemScript.selectedItem, this);
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