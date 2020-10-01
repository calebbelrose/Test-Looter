using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class CreateItemManager : MonoBehaviour
{

    public ItemDatabase itemDB;
    public ItemOverlayScript overlayScript;
    public ItemListManager listManager;
    public SortAndFilterManager sortManager;

    public Dropdown nameDropdown;
    private int selectedID = 0;
    private bool isRandomType = false;

    public Slider lvlSlider;
    public InputField lvlInput;
    public Toggle randomLvlToggle;
    private int selectedLvl = 1;
    private bool isRandomLvl = false;

    public Slider qualitySlider;
    public Toggle randomQualityToggle;
    public Text qualityText;
    public GameObject QualityPanel;
    private int selectedQuality = 0;
    private bool isRandomQuality = false;
    private List<StatBonus> selectedStats = new List<StatBonus>();

    public Button createButton;
    public Button randomButton;
    public Toggle addToListToggle;
    public bool willAddToList = false;

    private void Start()
    {
        nameDropdown.AddOptions(itemDB.TypeNameList);//gets ann itemname form database
    }

    public void ButtonEnter()
    {
        overlayScript.UpdateOverlay2(new ItemClass(selectedID, selectedLvl, selectedQuality, selectedStats), !isRandomType, !isRandomLvl, !isRandomQuality);
    }
    public void ButtonExit()
    {
        overlayScript.UpdateOverlay(null);
    }

    #region stats
    public void OnStatChange(StatName statName, int value)
    {
        StatBonus newStat = new StatBonus(statName, value);
        int index = selectedStats.IndexOf(newStat);

        if (index == -1)
            selectedStats.Add(newStat);
        else
            selectedStats[index].BonusValue = value;
    }

    #endregion

    #region type dropdown
    public void OnNameDropdownChange(int index)
    {
        selectedID = index;
    }
    public void OnRandomTypeToggle(bool isToggled)
    {
        nameDropdown.interactable = !isToggled;
        isRandomType = isToggled;
        if (!isToggled)
        {
            selectedID = nameDropdown.value;
        }
    }
    #endregion

    #region lvl slider

    public void OnLvlSliderChange(float value)
    {
        selectedLvl = (int)value;
        lvlInput.text = value.ToString();
    }
    public void OnLvlInputChange(string value)
    {
        if (value != "")
        {
            selectedLvl = int.Parse(value);
        }
        else
        {
            selectedLvl = 0;
        }
        lvlSlider.value = selectedLvl;
    }
    public void OnRandomLvlToggleChange(bool isToggled)
    {
        lvlSlider.interactable = !isToggled;
        lvlInput.interactable = !isToggled;
        isRandomLvl = isToggled;
        if (!isToggled)
        {
            selectedLvl = (int)lvlSlider.value;
        }
    }
    #endregion

    #region quality slider 
    //could be dropdown
    public void OnQualitySliderChange(float value)
    {
        selectedQuality = (int)value;
        qualityText.text = ItemDatabase.Instance.Qualities[selectedQuality].Name;
    }
    public void OnRandomQualityToggleChange(bool isToggled)
    {
        qualitySlider.interactable = !isToggled;
        Image image = QualityPanel.GetComponent<Image>();
        image.color = isToggled ? new Color32(200, 200, 200, 128) : new Color32(255, 255, 255, 255);
        isRandomQuality = isToggled;
    }

    #endregion

    #region button events
    public void CreateItem()//for create button
    {
        if (ItemScript.selectedItem == null)
        {
            if (isRandomType) { selectedID = Random.Range(0, ItemDatabase.Instance.TypeNameList.Count); }
            if (isRandomLvl) { selectedLvl = Random.Range(1, CombatController.MaxLevel + 1); }
            if (isRandomQuality) { selectedQuality = Random.Range(0, ItemDatabase.Instance.Qualities.Count); }
            ItemClass newItem = new ItemClass(selectedID, selectedLvl, selectedQuality, selectedStats);
            SpawnOrAddItem(newItem);
        }
    }
    

    private void SpawnOrAddItem(ItemClass passedItem)
    {
        ItemScript itemObject = ItemDatabase.Instance.ItemEquipPool.GetObject().GetComponent<ItemScript>();
        itemObject.SetItemObject(passedItem);
        ItemScript.SetSelectedItem(itemObject);
    }
    public void AddToListToggle(bool isToggled)
    {
        willAddToList = isToggled;
    }
    #endregion
}