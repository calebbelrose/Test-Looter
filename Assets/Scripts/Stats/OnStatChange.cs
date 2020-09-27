using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnStatChange : MonoBehaviour
{
    public InputField InputField;
    public CreateItemManager CreateItemManager;
    public StatName StatName;

    void Awake()
    {
        InputField.onValueChanged.AddListener((value) => CreateItemManager.OnStatChange(StatName, int.Parse(value)));
    }
}
