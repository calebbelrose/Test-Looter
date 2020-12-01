using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOverlayScript : MonoBehaviour {

    [SerializeField] private Text nameText, lvlText, qualityText;
    [SerializeField] private Image Icon;
    [SerializeField] private RectTransform IconRect, BackgroundRect, RectTransform;
    [SerializeField] private Sprite nullSprite;
    [SerializeField] private GameObject EmptySlotPrefab, StatPrefab;
    [SerializeField] private Transform statParent;

    private Vector3 offset = new Vector3(25f, -25f, 0f);

    //Sets up overlay
    private void Start()
    {
        SlotSectorScript.OverlayScript = this;
        UpdateOverlay(null);
    }

    //Moves overlay to cursor
    void Update()
    {
        float width, height, distPastX, distPastY;
        Vector3 pos = Input.mousePosition;
        Vector3[] corners = new Vector3[4];

        RectTransform.GetWorldCorners(corners);
        width = corners[2].x - corners[0].x;
        height = corners[1].y - corners[0].y;

        distPastX = pos.x + width + offset.x - Screen.width;

        if (distPastX > 0)
            pos = new Vector3(pos.x - distPastX, pos.y, pos.z);

        distPastY = pos.y + offset.y - height;

        if (distPastY < 0)
            pos = new Vector3(pos.x, pos.y - distPastY, pos.z);

        transform.position = pos + offset;
    }

    //Updates overlay information
    public void UpdateOverlay(ItemClass item)
    {
        if (item != null)
        {
            int totalSize = item.Size.x * item.Size.y;

            gameObject.SetActive(true);

            foreach (Transform child in BackgroundRect)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < totalSize; i++)
                Instantiate(EmptySlotPrefab, BackgroundRect);

            foreach (Transform child in statParent)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < item.StatBonuses.Count; i++)
            {
                Transform statTransform = Instantiate(StatPrefab, statParent).transform;

                statTransform.GetChild(0).GetComponent<Text>().text = item.StatBonuses[i].StatName.ToString();
                statTransform.GetChild(1).GetComponent<Text>().text = item.StatBonuses[i].BonusValue.ToString();
            }

            BackgroundRect.sizeDelta = new Vector2(item.Size.x * 50, item.Size.y * 50);
            nameText.text = item.TypeName;
            lvlText.text = "Lvl: " + item.Level;
            qualityText.text = item.Quality.Name;
            qualityText.color = item.Quality.Colour;
            Icon.color = new Color32(255, 255, 255, 255);
            Icon.sprite = item.Icon;
            IconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Size.x * InvenGridScript.SlotSize);
            IconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Size.y * InvenGridScript.SlotSize);
        }
        else
            gameObject.SetActive(false);
    }
}
