using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashButtonScript : MonoBehaviour {

    public Button button;
    public ObjectPoolScript itemEquipPool;

    private void Start()
    {
        button.onClick.AddListener(DestroyItem);
    }
    
    private void DestroyItem()
    {
        if (ItemScript.selectedItem != null)
        {
            InvenGridManager.Instance.RemoveSelectedButton();
            itemEquipPool.ReturnObject(ItemScript.selectedItem.gameObject);
            ItemScript.ResetSelectedItem();
        }
    }
}
