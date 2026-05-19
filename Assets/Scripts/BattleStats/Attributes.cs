using System;
using System.Collections.Generic;
using System.Linq;
using Drafts;
using UnityEngine;

public enum Attribute {
    Str,
    Int,
    Dex,
    Vit,
    Spt
}

public interface IAttributes {
    int this[Attribute a] { get; }
}

[Serializable]
public class Attributes : IAttributes, IThreeColumnsDrawer {
    public static int Count => All.Count;
    public static IReadOnlyList<Attribute> All { get; } = Enum.GetValues(typeof(Attribute)).OfType<Attribute>().ToList();

    [Label("STR")] public int strength;
    [Label("INT")] public int intelligence;
    [Label("DEX")] public int dexterity;
    [Label("VIT")] public int vitality;
    [Label("SPT")] public int spirit;

    public int Sum() => strength + intelligence + dexterity + vitality + spirit;

    public void Set(IAttributes other) {
        foreach (var a in All)
            this[a] = other[a];
    }

    public int this[Attribute a] {
        get => a switch {
            Attribute.Str => strength,
            Attribute.Int => intelligence,
            Attribute.Dex => dexterity,
            Attribute.Vit => vitality,
            Attribute.Spt => spirit,
            _ => 0,
        };
        set {
            switch (a) {
                case Attribute.Str: strength = value; break;
                case Attribute.Int: intelligence = value; break;
                case Attribute.Dex: dexterity = value; break;
                case Attribute.Vit: vitality = value; break;
                case Attribute.Spt: spirit = value; break;
            }
        }
    }
}