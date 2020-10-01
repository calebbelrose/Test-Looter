using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

    public TextAsset dbFile;
    public SortAndFilterManager safm;
    public ObjectPoolScript ItemEquipPool;
    public CombatController CombatController;

    public List<string> TypeNameList = new List<string>();
    public List<Quality> Qualities;
    private static ItemDatabase instance = null;
    public Transform LootParent;
    public List<ItemData> dbList = new List<ItemData>();

    public static ItemDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.transform.parent.parent.parent);
        LoadDb(dbFile);
    }

    private void LoadDb(TextAsset csvFile)
    {
        string[][] grid = CsvReadWrite.LoadTextFile(csvFile);
        for (int i = 1; i < grid.Length; i++)
        {
            ItemData row = new ItemData();
            row.GlobalID = Int32.Parse(grid[i][0]);
            row.CategoryID = Int32.Parse(grid[i][1]);
            row.CategoryName = (CategoryName)Enum.Parse(typeof(CategoryName), grid[i][2]);
            row.TypeID = Int32.Parse(grid[i][3]);
            row.TypeName = grid[i][4];
            TypeNameList.Add(row.TypeName);
            row.Size = new IntVector2(Int32.Parse(grid[i][5]), Int32.Parse(grid[i][6]));
            row.Icon = Resources.Load<Sprite>("ItemIcons/" + grid[i][4]);
            dbList.Add(row);
        }
    }

    public void PassItemData(ref ItemClass item)
    {
        int ID = item.GlobalID;
        item.CategoryID = dbList[ID].CategoryID;
        item.CategoryName = dbList[ID].CategoryName;
        item.TypeID = dbList[ID].TypeID;
        item.TypeName = dbList[ID].TypeName;
        item.Size = dbList[ID].Size;
        item.Icon = dbList[ID].Icon;
    }
    public void RandomItem() //for random button
    {
        /*if (ItemScript.selectedItem == null)
        {
            List<Stat> stats = CharacterStats.Instance.Stats.OrderBy(x => Random.value).Take(Random.Range(0, System.Enum.GetValues(typeof(StatName)).Length)).ToList();

            for (int i = 0; i < stats.Count; i++)
                stats[i].Value = Random.Range(0, 1000);

            SpawnOrAddItem(new ItemClass(Random.Range(0, ItemDatabase.Instance.TypeNameList.Count), Random.Range(1, CharacterStats.MaxLevel + 1), Random.Range(0, ItemDatabase.Instance.Qualities.Count), stats));
        }*/
        SpawnLoot(Vector3.zero + Vector3.forward * 15);
    }

    private void SpawnLoot(Vector3 location)
    {
        List<Stat> stats = CombatController.Stats.OrderBy(x => UnityEngine.Random.value).Take(UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(StatName)).Length)).ToList();
        List<StatBonus> statBonuses = new List<StatBonus>();
        ItemClass newItem = new ItemClass();

        for (int i = 0; i < stats.Count; i++)
            statBonuses.Add(new StatBonus(stats[i].StatName, (CombatController.Level - 1) * 10 + UnityEngine.Random.Range(0, 9)));
        
        GameObject newLoot = Instantiate(Resources.Load("Loot/" + newItem.TypeName) as GameObject);
        newLoot.GetComponent<Loot>().item = newItem;
        newLoot.transform.position = location;
    }

    //create find item funtions later

    //*from CsvParser2*
    //public Row Find_ItemTypeID(string find)
    //{
    //	return rowList.Find(x => x.GlobalID.ToString() == find);
    //}
    //public List<Row> FindAll_ItemTypeID(string find)
    //{
    //	return rowList.FindAll(x => x.GlobalID.ToString() == find);
    //}
    //public Row Find_ItemTypeName(string find)
    //{
    //	return rowList.Find(x => x.TypeName == find);
    //}
    //public List<Row> FindAll_ItemTypeName(string find)
    //{
    //	return rowList.FindAll(x => x.TypeName == find);
    //}
    //public Row Find_SizeX(string find)
    //{
    //	return rowList.Find(x => x.SizeX == find);
    //}
    //public List<Row> FindAll_SizeX(string find)
    //{
    //	return rowList.FindAll(x => x.SizeX == find);
    //}
    //public Row Find_SizeY(string find)
    //{
    //	return rowList.Find(x => x.SizeY == find);
    //}
    //public List<Row> FindAll_SizeY(string find)
    //{
    //	return rowList.FindAll(x => x.SizeY == find);
    //}
}