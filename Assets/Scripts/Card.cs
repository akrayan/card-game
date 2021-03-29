using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CardColor
{
    Hearth,
    Diamond,
    Club,
    Spade
}

public enum Value
{
    Ace,
    Jack = 11,
    Queen,
    King
}

[Serializable]
public struct Card
{
    public CardColor Color;
    public ushort Value;

    public Card(CardColor color, ushort value) { Color = color; Value = value; }
    /*public static bool operator ==(Card a, Card b) { return a.Color == b.Color && a.Value == b.Value; }
    public static bool operator !=(Card a, Card b) { return a.Color != b.Color || a.Value != b.Value; }*/
}
