using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotSectorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int QuadNum {  get { return quadNum; } }

    [SerializeField] private int quadNum;
    [SerializeField] private SlotScript ParentSlotScript;

    public static SlotSectorScript SectorScript { get; private set; }
    public static ItemOverlayScript OverlayScript;

    //Highlight slot, display overlay if it has an item and change slot colour
    public void OnPointerEnter(PointerEventData eventData)
    {
        SectorScript = this;
        ParentSlotScript.InvenGridManager.HighlightedSlot = ParentSlotScript;

        if (ItemScript.selectedItem != null)
            ParentSlotScript.InvenGridManager.RefreshColor(true);

        if (ParentSlotScript.storedItemObject != null)
        {
            OverlayScript.UpdateOverlay(ParentSlotScript.storedItemClass);

            if (ItemScript.selectedItem == null)
                ParentSlotScript.InvenGridManager.ColorChangeLoop(ParentSlotScript.storedItemObject.item.Quality.Colour, ParentSlotScript.storedItemSize, ParentSlotScript.storedItemStartPos);
        }
    }

    //Reset overlay and slot colour
    public void OnPointerExit(PointerEventData eventData)
    {
        SectorScript = null;
        ParentSlotScript.InvenGridManager.HighlightedSlot = null;
        OverlayScript.UpdateOverlay(null);

        if (ItemScript.selectedItem != null)
            ParentSlotScript.InvenGridManager.RefreshColor(false);
        else if (ParentSlotScript.storedItemObject != null)
            ParentSlotScript.InvenGridManager.ColorChangeLoop(ParentSlotScript.storedItemObject.item.Quality.Colour, ParentSlotScript.storedItemSize, ParentSlotScript.storedItemStartPos);
    }
}