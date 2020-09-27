using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherItem : IEquatable<OtherItem>
{
    public ItemScript Item;
    public IntVector2 StartPosition;

    public OtherItem (ItemScript item, IntVector2 startPosition)
    {
        Item = item;
        StartPosition = startPosition;
    }

    public bool Equals(OtherItem other)
    {
        return Item == other.Item && StartPosition == other.StartPosition;
    }
}