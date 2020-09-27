using System.Collections.Generic;
using System.Linq;

public class Consumable : ItemClass
{
    public virtual void Consume()
    {

    }

    public Consumable(int id, int level, int quality, List<StatBonus> stats)
    {
        GlobalID = id;
        CategoryName = ItemDatabase.Instance.dbList[GlobalID].CategoryName;
        Level = level;
        Quality = ItemDatabase.Instance.Qualities[quality];
        StatBonuses = stats.ToList();
        SetItemValues(this);
    }
}
