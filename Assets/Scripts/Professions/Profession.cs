using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Profession
{
    public int Level { get; private set; } = 1;
    protected float experience = 0, experienceToNextLevel = 100;
    protected List<Recipe> recipes = new List<Recipe>();

    public void ListItems(Transform listParent)
    {
        foreach (Transform child in listParent)
            GameObject.Destroy(child.gameObject);

        foreach (Recipe recipe in recipes)
        {
            GameObject profession = GameObject.Instantiate(ItemDatabase.Instance.ProfessionPrefab, listParent);

            profession.transform.GetChild(0).GetComponent<Text>().text = ItemDatabase.Instance.ItemData(recipe.ItemID).TypeName;
            profession.GetComponent<Button>().onClick.AddListener(delegate { ItemDatabase.Instance.ProfessionItem.NewItem(recipe); });
        }
    }

    //Awards experience
    protected virtual void AwardExperience(float amount)
    {
        experience += amount;
        while (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.25);
            Level++;
        }
    }
}
