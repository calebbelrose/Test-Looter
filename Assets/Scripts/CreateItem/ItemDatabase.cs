using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public ObjectPoolScript ItemEquipPool { get { return itemEquipPool; } }
    public GameObject ProfessionPrefab { get { return professionPrefab; } }
    public ProfessionItem ProfessionItem { get { return professionItem; } }

    [SerializeField] private ObjectPoolScript itemEquipPool;
    [SerializeField] private List<LootDrop> RareLootDrops = new List<LootDrop>();
    [SerializeField] private List<Quality> Qualities;
    [SerializeField] private Transform LootParent;
    [SerializeField] private ObjectPoolScript ObjectPoolScript;
    [SerializeField] private GameObject professionPrefab;
    [SerializeField] private ProfessionItem professionItem;

    private List<ItemData> DBList = new List<ItemData>();
    private int totalLootDropWeights = 0;

    public static ItemDatabase Instance { get; private set; } = null;

    //Initializes singleton, loads database items from text file and prepares the rare loot drops for loot selection
    private void Awake()
    {
        string[] lines = File.ReadAllLines("./Assets/Scripts/CreateItem/DataBase.csv");

        if (Instance != null && Instance != this)
            Destroy(this.gameObject);

        Instance = this;
        DontDestroyOnLoad(transform.parent.parent.gameObject);
        RareLootDrops = RareLootDrops.OrderBy(x => x.Weight).ToList();

        foreach (string line in lines)
        {
            ItemData row = new ItemData();
            string[] data = line.Split(',');

            row.GlobalID = Int32.Parse(data[0]);
            row.CategoryID = Int32.Parse(data[1]);
            row.CategoryName = (CategoryName)Enum.Parse(typeof(CategoryName), data[2]);
            row.TypeName = data[3];
            row.Size = new IntVector2(Int32.Parse(data[4]), Int32.Parse(data[5]));
            row.Icon = Resources.Load<Sprite>("ItemIcons/" + data[3]);
            DBList.Add(row);
        }

        foreach (LootDrop drop in RareLootDrops)
            totalLootDropWeights += drop.Weight;
    }

    //Spawns a rare loot drop at the location based on the player's magic find chance
    public void SpawnLoot(Vector3 position, CombatController player)
    {
        List<Stat> stats = player.OrderedStats();
        List<StatBonus> statBonuses = new List<StatBonus>();
        ItemClass newItem;
        GameObject newLoot;

        for (int i = 0; i < stats.Count; i++)
            statBonuses.Add(new StatBonus(stats[i].StatName, (player.Level - 1) * 10 + UnityEngine.Random.Range(0, 9)));

        newItem = new ItemClass(RareLootDrops[UnityEngine.Random.Range(0, RareLootDrops.Count)].ItemID, player.Level, QualityIndex(player), statBonuses);
        newLoot = Instantiate(Resources.Load("Loot/" + newItem.TypeName) as GameObject);
        newLoot.GetComponent<Loot>().Item = newItem;
        newLoot.transform.position = position;
    }

    //Returns a random quality based on the player's magic find
    public int QualityIndex(CombatController player)
    {
        float[] qualityChances = new float[Qualities.Count];
        int index = 1;
        float random = UnityEngine.Random.value;

        qualityChances[1] = 1f;

        for (int i = 2; i < Qualities.Count; i++)
        {
            qualityChances[i] = Qualities[i].BaseChance * player.MagicFind;
            qualityChances[1] -= qualityChances[i];
        }

        while (index < qualityChances.Length && random > qualityChances[index])
            random -= qualityChances[index++];

        return index;
    }

    //Returns the quality at the specified index
    public Quality Quality(int index)
    {
        return Qualities[index];
    }

    //Returns the index of the specified quality
    public int Quality(Quality quality)
    {
        return Qualities.IndexOf(quality);
    }

    //Returns the data for the item at the specified index
    public ItemData ItemData(int index)
    {
        return DBList[index];
    }

    //Creates the specified loot with the set parent
    public GameObject CreateLoot(GameObject loot)
    {
        return Instantiate(loot, LootParent);
    }

    //Returns the specified object
    public void ReturnObject(GameObject toReturn)
    {
        ObjectPoolScript.ReturnObject(toReturn);
    }

        /*public void SpawnLoot(Vector3 position, int itemID, int amount)
        {
            int weightSum = 0;
            int index = 0;
            int roll;
            ItemClass newItem;
            GameObject newLoot;

            foreach (LootDrop drop in RareLootDrops)
                weightSum += drop.Weight;
            roll = UnityEngine.Random.Range(0, weightSum);

            while (index < RareLootDrops.Count && roll <= RareLootDrops[index].Weight)
                roll -= RareLootDrops[index].Weight;

            newItem = new ItemClass(itemID, );
            newLoot = Instantiate(Resources.Load("Loot/" + newItem.TypeName) as GameObject);
            newLoot.GetComponent<Loot>().item = newItem;
            newLoot.transform.position = position;
        }*/
    }