using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenGridManager : MonoBehaviour {

    public GameObject[,] slotGrid;
    public GameObject highlightedSlot;
    public Transform dropParent;
    [HideInInspector]
    public IntVector2 gridSize;

    public ItemListManager listManager;
    public GameObject selectedButton;
    public CharacterStats characterStats;

    private IntVector2 totalOffset, checkSize, checkStartPos;
    private List<OtherItem> otherItems = new List<OtherItem>();

    private int checkState;
    private bool isOverEdge = false;

    public ItemOverlayScript overlayScript;

    private static InvenGridManager instance;

    /* to do list
     * make the ColorChangeLoop on swap items take arrguements fron the other item, not hte private variables *1
     * transfer the CheckArea() and SlotCheck() into inside RefreshColor() *2
     * have *3 be local variables of CheckArea(). SwapItem() uses the variable, may need to rewrite.
     */
    public static InvenGridManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }


        private void Update()
    {
        if (highlightedSlot != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (ItemScript.selectedItem != null)
                {
                    if (!isOverEdge)
                    {
                        switch (checkState)
                        {
                            case 0: //store on empty slots
                                StoreItem(ItemScript.selectedItem);
                                ColorChangeLoop(ItemScript.selectedItem.item.Quality.Colour, ItemScript.selectedItemSize, totalOffset);
                                ItemScript.ResetSelectedItem();
                                RemoveSelectedButton();
                                break;
                            case 1: //swap items
                                ItemScript.SetSelectedItem(SwapItem(ItemScript.selectedItem));
                                SlotSectorScript.sectorScript.PosOffset();
                                ColorChangeLoop(Color.white, otherItems[0].Item.item.Size, otherItems[0].StartPosition); //*1
                                RefreshColor(true);
                                RemoveSelectedButton();
                                break;
                        }
                    }// retrieve items
                }
                else if (highlightedSlot.GetComponent<SlotScript>().isOccupied == true)
                {
                    ColorChangeLoop(Color.white, highlightedSlot.GetComponent<SlotScript>().storedItemSize, highlightedSlot.GetComponent<SlotScript>().storedItemStartPos);
                    ItemScript.SetSelectedItem(GetItem(highlightedSlot));
                    SlotSectorScript.sectorScript.PosOffset();
                    RefreshColor(true);
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                SlotScript slotScript = highlightedSlot.GetComponent<SlotScript>();
                if(slotScript.isOccupied == true)
                {
                    for (int y = 0; y < slotScript.storedItemSize.y; y++)
                    {
                        for (int x = 0; x < slotScript.storedItemSize.x; x++)
                            slotGrid[slotScript.storedItemStartPos.x + x, slotScript.storedItemStartPos.y + y].GetComponent<Image>().color = Color.white;
                    }
                    ItemScript item = GetItem(highlightedSlot);
                    
                    
                    characterStats.Equip(item, characterStats.EquipSlots.Find(x => x.CategoryName == item.item.CategoryName));
                    
                }
            }
        }
    }

    private void CheckArea(IntVector2 itemSize) //*2
    {
        IntVector2 halfOffset;
        IntVector2 overCheck;
        halfOffset.x = (itemSize.x - (itemSize.x % 2 == 0 ? 0 : 1)) / 2;
        halfOffset.y = (itemSize.y - (itemSize.y % 2 == 0 ? 0 : 1)) / 2;
        totalOffset = highlightedSlot.GetComponent<SlotScript>().gridPos - (halfOffset + SlotSectorScript.posOffset);
        checkStartPos = totalOffset;
        checkSize = itemSize;
        overCheck = totalOffset + itemSize;
        isOverEdge = false;
        //checks if item to stores is outside grid
        if (overCheck.x > gridSize.x)
        {
            checkSize.x = gridSize.x - totalOffset.x;
            isOverEdge = true;
        }
        if (totalOffset.x < 0)
        {
            checkSize.x = itemSize.x + totalOffset.x;
            checkStartPos.x = 0;
            isOverEdge = true;
        }
        if (overCheck.y > gridSize.y)
        {
            checkSize.y = gridSize.y - totalOffset.y;
            isOverEdge = true;
        }
        if (totalOffset.y < 0)
        {
            checkSize.y = itemSize.y + totalOffset.y;
            checkStartPos.y = 0;
            isOverEdge = true;
        }
    }
    private int SlotCheck(IntVector2 itemSize)//*2
    {
        otherItems.Clear();
        if (!isOverEdge)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    SlotScript instanceScript = slotGrid[checkStartPos.x + x, checkStartPos.y + y].GetComponent<SlotScript>();
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
        return 2; // check areaArea is over the grid
    }
    public void RefreshColor(bool enter)
    {
        if (enter)
        {
            CheckArea(ItemScript.selectedItemSize);
            checkState = SlotCheck(checkSize);
            switch (checkState)
            {
                case 0: ColorChangeLoop(Color.green, checkSize, checkStartPos); break; //no item in area
                case 1:
                    ColorChangeLoop(otherItems[0].Item.item.Quality.Colour, otherItems[0].Item.item.Size, otherItems[0].StartPosition); //1 item on area and can swap
                    ColorChangeLoop(Color.green, checkSize, checkStartPos);
                    break;
                default: ColorChangeLoop(Color.red, checkSize, checkStartPos); break; //invalid position (more than 2 items in area or area is outside grid)
            }
        }
        else //on pointer exit. resets colors
        {
            isOverEdge = false;
            //checkArea(); //commented out for performance. may cause bugs if not included

            ColorChangeLoop2(checkSize, checkStartPos);

            foreach (OtherItem otherItem in otherItems)
                ColorChangeLoop(otherItem.Item.item.Quality.Colour, otherItem.Item.item.Size, otherItem.StartPosition);
        }
    }
    public void ColorChangeLoop(Color32 color, IntVector2 size, IntVector2 startPos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = color;
            }
        }
    }
    public void ColorChangeLoop2(IntVector2 size, IntVector2 startPos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                SlotScript slot = slotGrid[startPos.x + x, startPos.y + y].GetComponent<SlotScript>();

                if (slot.isOccupied != false)
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = slot.storedItemObject.item.Quality.Colour;
                }
                else
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = Color.white;
                }
            }
        }
    }
    private void StoreItem(ItemScript item)
    {
        SlotScript instanceScript;
        IntVector2 itemSizeL = item.item.Size;
        for (int y = 0; y < itemSizeL.y; y++)
        {
            for (int x = 0; x < itemSizeL.x; x++)
            {
                //set each slot parameters
                instanceScript = slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<SlotScript>();
                instanceScript.storedItemObject = item;
                instanceScript.storedItemClass = item.item;
                instanceScript.storedItemSize = itemSizeL;
                instanceScript.storedItemStartPos = totalOffset;
                instanceScript.isOccupied = true;
                slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = Color.white;
            }
        }//set dropped parameters
        item.transform.SetParent(dropParent);
        item.gameObject.GetComponent<RectTransform>().pivot = Vector2.zero;
        item.transform.position = slotGrid[totalOffset.x, totalOffset.y].transform.position;
        item.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        overlayScript.UpdateOverlay(highlightedSlot.GetComponent<SlotScript>().storedItemClass);
    }
    private ItemScript GetItem(GameObject slotObject)
    {
        SlotScript slotObjectScript = slotObject.GetComponent<SlotScript>();
        ItemScript retItem = slotObjectScript.storedItemObject;
        IntVector2 tempItemPos = slotObjectScript.storedItemStartPos;
        IntVector2 itemSizeL = retItem.item.Size;

        SlotScript instanceScript;
        for (int y = 0; y < itemSizeL.y; y++)
        {
            for (int x = 0; x < itemSizeL.x; x++)
            {
                //reset each slot parameters
                instanceScript = slotGrid[tempItemPos.x + x, tempItemPos.y + y].GetComponent<SlotScript>();
                instanceScript.storedItemObject = null;
                instanceScript.storedItemSize = IntVector2.Zero;
                instanceScript.storedItemStartPos = IntVector2.Zero;
                instanceScript.storedItemClass = null;
                instanceScript.isOccupied = false;
            }
        }//returned item set item parameters
        retItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retItem.GetComponent<CanvasGroup>().alpha = 0.5f;
        retItem.transform.position = Input.mousePosition;
        overlayScript.UpdateOverlay(null);
        return retItem;
    }
    private ItemScript SwapItem(ItemScript item)
    {
        ItemScript tempItem;
        tempItem = GetItem(slotGrid[otherItems[0].StartPosition.x, otherItems[0].StartPosition.y]);
        StoreItem(item);
        return tempItem;
    }

    public void RemoveSelectedButton()
    {
        if (selectedButton != null)
        {
            listManager.RevomeItemFromList(selectedButton.GetComponent<ItemButtonScript>().item);
            listManager.RemoveButton(selectedButton);
            listManager.sortManager.SortAndFilterList();
            selectedButton = null;

        }
    }
}