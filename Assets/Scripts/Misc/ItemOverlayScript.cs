using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOverlayScript : MonoBehaviour {

    public Text nameText, lvlText, qualityText;
    public Image Icon;
    public RectTransform IconBackground;
    public Sprite nullSprite;
    public GameObject EmptySlotPrefab;
    public GameObject StatPrefab;
    public Transform statParent;

    private Vector3 offset = new Vector3(25f, -25f, 0f);
    private float slotSize;

    private void Start()
    {
        ItemButtonScript.overlayScript = this;
        SlotSectorScript.overlayScript = this;
        UpdateOverlay(null);
        slotSize = InvenGridScript.Instance.slotSize;
    }
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        Vector3[] corners = new Vector3[4];

        ((RectTransform)transform).GetWorldCorners(corners);
        float width = corners[2].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        float distPastX = pos.x + width + offset.x - Screen.width;
        if (distPastX > 0)
            pos = new Vector3(pos.x - distPastX, pos.y, pos.z);
        float distPastY = pos.y + offset.y - height;
        if (distPastY < 0)
            pos = new Vector3(pos.x, pos.y - distPastY, pos.z);

        transform.position = pos + offset;
    }

    public void UpdateOverlay(ItemClass item)
    {
        if (item != null)
        {
            int totalSize = item.Size.x * item.Size.y;

            gameObject.SetActive(true);

            foreach (Transform child in IconBackground)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < totalSize; i++)
                Instantiate(EmptySlotPrefab, IconBackground);

            foreach (Transform child in statParent)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < item.StatBonuses.Count; i++)
            {
                Transform statTransform = Instantiate(StatPrefab, statParent).transform;
                statTransform.GetChild(0).GetComponent<Text>().text = item.StatBonuses[i].StatName.ToString();
                statTransform.GetChild(1).GetComponent<Text>().text = item.StatBonuses[i].BonusValue.ToString();

            }

            IconBackground.sizeDelta = new Vector2(item.Size.x * 50, item.Size.y * 50);
            nameText.text = item.TypeName;
            lvlText.text = "Lvl: " + item.Level;
            qualityText.text = item.Quality.Name;
            qualityText.color = item.Quality.Colour;
            Icon.color = new Color32(255, 255, 255, 255);
            Icon.sprite = item.Icon;
            RectTransform rect = Icon.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Size.x * slotSize);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Size.y * slotSize);
        }
        else
            gameObject.SetActive(false);
    }

    public void UpdateOverlay2(ItemClass item, bool ID, bool lvl, bool quality)
    {
        if (item != null)
        {
            int totalSize = item.Size.x * item.Size.y;

            gameObject.SetActive(true);

            foreach (Transform child in IconBackground)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < totalSize; i++)
                Instantiate(EmptySlotPrefab, IconBackground);

            nameText.text = ID ? item.TypeName : "***";
            lvlText.text = lvl ? "Lvl: " + item.Level : "***";
            qualityText.text = quality ? item.Quality.Name : "***";
            Icon.color =new Color32(255, 255, 255, 255);
            Icon.sprite = ID ? item.Icon : nullSprite;
            qualityText.color = item.Quality.Colour;
            IntVector2 size = ID ? item.Size : new IntVector2(4, 4);
            IconBackground.sizeDelta = new Vector2(item.Size.x * 50, item.Size.y * 50);
            RectTransform rect = Icon.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x * slotSize);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y * slotSize);
        }
        else
            gameObject.SetActive(false);
    }
}
