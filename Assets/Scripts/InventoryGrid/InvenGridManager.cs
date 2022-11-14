using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InvenGridManager : MonoBehaviour
{
    public SlotScript[,] SlotGrid;
    public SlotScript HighlightedSlot;
    public Transform DragParent { get { return dragParent; } }

    [SerializeField] private CharacterStats CharacterStats;
    [SerializeField] private Transform DropParent;
    [SerializeField] private ItemOverlayScript overlayScript;
    [SerializeField] private Transform dragParent;

    private IntVector2 totalOffset, checkSize, checkStartPos;
    private List<OtherItem> otherItems = new List<OtherItem>();
    private int checkState;
    private bool isOverEdge = false;

    public static IntVector2 GridSize;
    public static InvenGridManager Instance { get; private set; } = null;

    //Sets up the singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);

        Instance = this;
        DontDestroyOnLoad(transform.parent.parent.gameObject);
    }

    //Loads inventory and hides the canvas
    private void Start()
    {
        LoadInventory();
        transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (HighlightedSlot != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //If an item is picked up
                if (ItemScript.selectedItem != null)
                {
                    if (!isOverEdge)
                    {
                        switch (checkState)
                        {
                            case 0: //Store on empty slots
                                StoreItem(ItemScript.selectedItem);
                                SaveItem(ItemScript.selectedItem);
                                ColorChangeLoop(ItemScript.selectedItem.item.Quality.Colour, ItemScript.selectedItemSize, totalOffset);
                                ItemScript.ResetSelectedItem();
                                break;
                            case 1: //Swap items
                                ItemScript.SetSelectedItem(SwapItem(ItemScript.selectedItem));
                                ColorChangeLoop(Color.white, otherItems[0].Item.item.Size, otherItems[0].StartPosition); //*1
                                RefreshColor(true);
                                break;
                        }
                    }
                }
                //If the slot has an item
                else
                {
                    if (HighlightedSlot.isOccupied)
                    {
                        ColorChangeLoop(Color.white, HighlightedSlot.storedItemSize, HighlightedSlot.storedItemStartPos);
                        ItemScript.SetSelectedItem(GetItem(HighlightedSlot));
                        RefreshColor(true);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                SlotScript slotScript = HighlightedSlot;

                //Uses item in slot
                if (slotScript.isOccupied == true)
                {
                    ItemScript itemScript = GetItem(HighlightedSlot);

                    for (int y = 0; y < slotScript.storedItemSize.y; y++)
                    {
                        for (int x = 0; x < slotScript.storedItemSize.x; x++)
                            SlotGrid[slotScript.storedItemStartPos.x + x, slotScript.storedItemStartPos.y + y].GetComponent<Image>().color = Color.white;
                    }

                    CharacterStats.Equip(itemScript);
                    CharacterStats.SaveEquipment(itemScript);
                }
            }
        }
    }

    //Returns true if the loot was stored otherwise returns false
    public bool StoreLoot(ItemScript itemScript)
    {
        //Loops over all of the slots to find a space big enough for the item
        for (int x = 0; x < SlotGrid.GetLength(0) - itemScript.item.Size.x; x++)
        {
            for (int y = 0; y < SlotGrid.GetLength(1) - itemScript.item.Size.y; y++)
            {
                int i = 0;
                bool stillEmpty = true;

                while(i < itemScript.item.Size.x && stillEmpty)
                {
                    for (int j = 0; j < itemScript.item.Size.y; j++)
                    {
                        Debug.Log(SlotGrid[x + i, y + j].gridPos);
                        if (SlotGrid[x + i, y + j].isOccupied)
                        {
                            stillEmpty = false;
                            break;
                        }
                    }

                    i++;
                }

                // Stores the item if there's a space big enough
                if (stillEmpty)
                {
                    totalOffset = SlotGrid[x, y].gridPos;
                    StoreItem(itemScript);
                    SaveItem(itemScript);
                    itemScript.Rect.localScale = Vector3.one;
                    ColorChangeLoop(itemScript.item.Quality.Colour, itemScript.item.Size, totalOffset);
                    return true;
                }
            }
        }

        return false;
    }

    //Checks if item to store is outside grid
    private void CheckArea(IntVector2 itemSize, SlotScript slotScript) //*2
    {
        IntVector2 overCheck;

        totalOffset = slotScript.gridPos - Offset(itemSize);
        checkStartPos = totalOffset;
        checkSize = itemSize;
        overCheck = totalOffset + itemSize;
        isOverEdge = false;

        if (overCheck.x > GridSize.x)
        {
            checkSize.x = GridSize.x - totalOffset.x;
            isOverEdge = true;
        }
        if (totalOffset.x < 0)
        {
            checkSize.x = itemSize.x + totalOffset.x;
            checkStartPos.x = 0;
            isOverEdge = true;
        }
        if (overCheck.y > GridSize.y)
        {
            checkSize.y = GridSize.y - totalOffset.y;
            isOverEdge = true;
        }
        if (totalOffset.y < 0)
        {
            checkSize.y = itemSize.y + totalOffset.y;
            checkStartPos.y = 0;
            isOverEdge = true;
        }
    }

    //Checks how many items the picked up item overlaps with
    private int SlotCheck(IntVector2 itemSize)//*2
    {
        otherItems.Clear();

        if (!isOverEdge)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    SlotScript instanceScript = SlotGrid[checkStartPos.x + x, checkStartPos.y + y];

                    if (instanceScript.isOccupied)
                    {
                        OtherItem otherItem = new OtherItem(instanceScript.storedItemObject, instanceScript.storedItemStartPos);

                        if (!otherItems.Contains(otherItem))
                            otherItems.Add(otherItem);
                    }
                }
            }
            return otherItems.Count;
        }
        return 2;
    }

    //Changes the colour of the slot based on how many items are overlapping or if the picked up item is over the edge
    public void RefreshColor(bool enter)
    {
        if (enter)
        {
            CheckArea(ItemScript.selectedItemSize, HighlightedSlot);
            checkState = SlotCheck(checkSize);

            switch (checkState)
            {
                case 0: ColorChangeLoop(Color.green, checkSize, checkStartPos); break;
                case 1:
                    ColorChangeLoop(otherItems[0].Item.item.Quality.Colour, otherItems[0].Item.item.Size, otherItems[0].StartPosition);
                    ColorChangeLoop(Color.green, checkSize, checkStartPos);
                    break;
                default: ColorChangeLoop(Color.red, checkSize, checkStartPos); break;
            }
        }
        else
        {
            isOverEdge = false;
            ColorChangeLoop2(checkSize, checkStartPos);

            foreach (OtherItem otherItem in otherItems)
                ColorChangeLoop(otherItem.Item.item.Quality.Colour, otherItem.Item.item.Size, otherItem.StartPosition);
        }
    }

    //Changes slots in an area to specified colour
    public void ColorChangeLoop(Color32 color, IntVector2 size, IntVector2 startPos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
                SlotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = color;
        }
    }

    //Changes slots in an area to a colour based on what item is in the slot
    public void ColorChangeLoop2(IntVector2 size, IntVector2 startPos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                SlotScript slot = SlotGrid[startPos.x + x, startPos.y + y];

                if (slot.isOccupied != false)
                    SlotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = slot.storedItemObject.item.Quality.Colour;
                else
                    SlotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = Color.white;
            }
        }
    }

    //Stores item in slot
    private void StoreItem(ItemScript itemScript)
    {
        SlotScript instanceScript;
        IntVector2 itemSizeL = itemScript.item.Size;

        for (int y = 0; y < itemSizeL.y; y++)
        {
            for (int x = 0; x < itemSizeL.x; x++)
            {
                instanceScript = SlotGrid[totalOffset.x + x, totalOffset.y + y];
                instanceScript.storedItemObject = itemScript;
                instanceScript.storedItemClass = itemScript.item;
                instanceScript.storedItemSize = itemSizeL;
                instanceScript.storedItemStartPos = totalOffset;
                instanceScript.isOccupied = true;
                SlotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = Color.white;
            }
        }

        itemScript.transform.SetParent(DropParent);
        itemScript.Rect.pivot = Vector2.zero;
        itemScript.transform.position = SlotGrid[totalOffset.x, totalOffset.y].transform.position;
        itemScript.CanvasGroup.alpha = 1f;
        overlayScript.UpdateOverlay(itemScript.item);
    }

    //Gets item in slot
    private ItemScript GetItem(SlotScript slotScript)
    {
        ItemScript retItem = slotScript.storedItemObject;
        IntVector2 tempItemPos = slotScript.storedItemStartPos;
        IntVector2 itemSizeL = retItem.item.Size;
        SlotScript instanceScript;

        File.WriteAllLines("./Assets/Scripts/CreateItem/SavedInventory.csv", File.ReadLines("./Assets/Scripts/CreateItem/SavedInventory.csv").Where(line => GetPosition(line) != tempItemPos).ToList());

        for (int y = 0; y < itemSizeL.y; y++)
        {
            for (int x = 0; x < itemSizeL.x; x++)
            {
                instanceScript = SlotGrid[tempItemPos.x + x, tempItemPos.y + y];
                instanceScript.storedItemObject = null;
                instanceScript.storedItemSize = IntVector2.Zero;
                instanceScript.storedItemStartPos = IntVector2.Zero;
                instanceScript.storedItemClass = null;
                instanceScript.isOccupied = false;
            }
        }
        retItem.Rect.pivot = new Vector2(0.5f, 0.5f);
        retItem.CanvasGroup.alpha = 0.5f;
        retItem.transform.position = Input.mousePosition;
        overlayScript.UpdateOverlay(null);
        return retItem;
    }

    //Swaps picked up item with specified item
    private ItemScript SwapItem(ItemScript item)
    {
        ItemScript tempItem;

        tempItem = GetItem(SlotGrid[otherItems[0].StartPosition.x, otherItems[0].StartPosition.y]);
        StoreItem(item);
        SaveItem(item);
        return tempItem;
    }
    
    //Saves the item to its inventory slot
    private void SaveItem(ItemScript itemScript)
    {
        StreamWriter sw;
        sw = File.AppendText("./Assets/Scripts/CreateItem/SavedInventory.csv");
        sw.WriteLine(totalOffset.x + "," + totalOffset.y + "," + itemScript.item.GlobalID + "," + itemScript.item.Level + "," + ItemDatabase.Instance.Quality(itemScript.item.Quality) + itemScript.item.StatString);
        sw.Close();
    }

    //Gets the position of the item in the inventory
    private IntVector2 GetPosition(string line)
    {
        string[] data = line.Split(',');

        return new IntVector2(int.Parse(data[0]), int.Parse(data[1]));
    }

    //Loads inventory
    private void LoadInventory()
    {
        string[] lines = File.ReadAllLines("./Assets/Scripts/CreateItem/SavedInventory.csv");

        for(int i = 0; i < lines.Length; i++)
        {
            List<StatBonus> statBonuses = new List<StatBonus>();
            string[] data = lines[i].Split(',');
            ItemClass item;
            ItemScript itemScript = ItemDatabase.Instance.ItemEquipPool.GetItemScript();
            int id = int.Parse(data[2]);

            if (IsStackable(ItemDatabase.Instance.ItemData(id).CategoryName))
            {
                item = new ItemClass(id);
                itemScript.SetQuantity(int.Parse(data[3]));
            }
            else
            {
                for (int j = 5; j < data.Length; j += 2)
                    statBonuses.Add(new StatBonus((StatName)int.Parse(data[j]), int.Parse(data[j + 1])));

                item = new ItemClass(id, int.Parse(data[3]), int.Parse(data[4]), statBonuses);
            }
            itemScript.SetItemObject(item);
            totalOffset = SlotGrid[int.Parse(data[0]), int.Parse(data[1])].gridPos;
            StoreItem(itemScript);
            itemScript.Rect.localScale = Vector3.one;
            ColorChangeLoop(item.Quality.Colour, item.Size, totalOffset);
        }
    }

    //Whether the object can be stacked or not
    private bool IsStackable(CategoryName categoryName)
    {
        return categoryName == CategoryName.Material || categoryName == CategoryName.Consumable;
    }

    //Offset of the object
    private IntVector2 Offset(IntVector2 itemSize)
    {
        return new IntVector2((itemSize.x - (itemSize.x % 2 == 0 ? 0 : 1)) / 2, (itemSize.y - (itemSize.y % 2 == 0 ? 0 : 1)) / 2);
    }
}