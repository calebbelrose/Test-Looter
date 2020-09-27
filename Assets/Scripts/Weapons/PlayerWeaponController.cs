using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform PlayerHand;
    public List<ItemScript> Equipped = new List<ItemScript>();
    public CharacterStats characterStats;

    public void Equip(ItemClass itemToEquip)
    {
        ItemScript equippedItem = Equipped.Find(x => x.item.CategoryName == itemToEquip.CategoryName);

        if (equippedItem != null)
        {
            characterStats.RemoveStatBonus(equippedItem.item.StatBonuses);
            Equipped.Remove(equippedItem);
            Destroy(equippedItem.gameObject);
        }

        equippedItem = Instantiate(Resources.Load<GameObject>(equippedItem.item.CategoryName + "/" + equippedItem.item.TypeName), PlayerHand.position, PlayerHand.rotation).GetComponent<ItemScript>();
        characterStats.AddStatBonus(equippedItem.item.StatBonuses);
        Equipped.Add(equippedItem);
        equippedItem.transform.parent = PlayerHand;
    }

    void Attack()
    {

    }
}
