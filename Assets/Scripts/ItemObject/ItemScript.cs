using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour, IPointerClickHandler
{
    public ItemClass item { get; private set; }
    public CanvasGroup CanvasGroup{ get { return canvasGroup; } }
    public RectTransform Rect { get { return rect; } }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image Image;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Text text;

    private int quantity;

    public static ItemScript selectedItem;
    public static IntVector2 selectedItemSize;
    public static bool isDragging = false;

    //Sets up the object
    public void SetItemObject(ItemClass passedItem)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, passedItem.Size.x * InvenGridScript.SlotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, passedItem.Size.y * InvenGridScript.SlotSize);
        item = passedItem;
        Image.sprite = passedItem.Icon;
    }

    public void SetQuantity(int _quantity)
    {
        quantity = _quantity;
        text.text = quantity.ToString();
    }

    //Pick up item
    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectedItem(this);
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0.5f;
    }

    //Moves picked up item to cursor
    private void Update()
    {
        if (isDragging)
            selectedItem.transform.position = Input.mousePosition;
    }

    //Sets picked up item
    public static void SetSelectedItem(ItemScript obj)
    {
        selectedItem = obj;
        selectedItemSize = obj.item.Size;
        isDragging = true;
        obj.transform.SetParent(InvenGridManager.Instance.DragParent);
        obj.rect.localScale = Vector3.one;
    }

    //Resets picked up item
    public static void ResetSelectedItem()
    {
        selectedItem = null;
        selectedItemSize = IntVector2.Zero;
        isDragging = false;
    }
}