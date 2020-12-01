using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public int RequiredLevel { get; private set; }
    public int ItemID { get; private set; }
    private float experienceToAward;
    private int[] requirements;

    public Recipe(int _requiredLevel, int _itemID, float _experienceToAward, int[] _requirements)
    {
        RequiredLevel = _requiredLevel;
        ItemID = _itemID;
        experienceToAward = _experienceToAward;
        requirements = _requirements;
    }

    public Recipe(int _requiredLevel, int _itemID, float _experienceToAward)
    {
        RequiredLevel = _requiredLevel;
        ItemID = _itemID;
        experienceToAward = _experienceToAward;
    }
}
