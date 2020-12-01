using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfessionItem : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private CombatController player;
    [SerializeField] private Transform DragParent;

    private int itemID = -1;

    public void NewItem(Recipe recipe)
    {
        Image.sprite = ItemDatabase.Instance.ItemData(recipe.ItemID).Icon;
        Image.rectTransform.sizeDelta = new Vector2(ItemDatabase.Instance.ItemData(recipe.ItemID).Size.x * InvenGridScript.SlotSize, ItemDatabase.Instance.ItemData(recipe.ItemID).Size.y * InvenGridScript.SlotSize);
        itemID = recipe.ItemID;
    }

    public void ClearItem()
    {
        Image.sprite = null;
        itemID = -1;
    }

    public void CraftItem()
    {
        if (itemID != -1)
        {
            List<Stat> stats = player.OrderedStats();
            List<StatBonus> statBonuses = new List<StatBonus>();
            ItemScript newItem;

            for (int i = 0; i < stats.Count; i++)
                statBonuses.Add(new StatBonus(stats[i].StatName, (player.Level - 1) * 10 + UnityEngine.Random.Range(0, 9)));

            newItem = ItemDatabase.Instance.ItemEquipPool.GetItemScript();
            newItem.SetItemObject(new ItemClass(itemID, player.Level, ItemDatabase.Instance.QualityIndex(player), statBonuses));
            ItemScript.SetSelectedItem(newItem);
        }
    }
}