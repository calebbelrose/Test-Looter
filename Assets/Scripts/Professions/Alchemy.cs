using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemy : Crafting
{
    public Alchemy()
    {
        recipes.Add(new Recipe(0, 0, 10, new int[0]));
    }
}
