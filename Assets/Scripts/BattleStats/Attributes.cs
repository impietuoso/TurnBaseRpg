using System;
using System.Collections.Generic;
using System.Linq;

public enum Attribute {
    Str,
    Int,
    Dex,
    Vit,
    Spt
}

public interface IAttributes {
    int this[Attribute attribute] { get; }
}

[Serializable]
public class Attributes : IAttributes {
    public static int Count => All.Count;
    public static IReadOnlyList<Attribute> All { get; } = Enum.GetValues(typeof(Attribute)).OfType<Attribute>().ToList();

    public int strength = 5;
    public int intelligence = 5;
    public int dexterity = 5;
    public int vitality = 5;
    public int spirit = 5;

    public int Sum() => strength + intelligence + dexterity + vitality + spirit;

    public void Set(IAttributes other) {
        foreach (var a in All)
            this[a] = other[a];
    }

    public int this[Attribute attribute] {
        get => attribute switch {
            Attribute.Str => strength,
            Attribute.Int => intelligence,
            Attribute.Dex => dexterity,
            Attribute.Vit => vitality,
            Attribute.Spt => spirit,
            _ => 0,
        };
        set {
            switch (attribute) {
                case Attribute.Str: strength = value; break;
                case Attribute.Int: intelligence = value; break;
                case Attribute.Dex: dexterity = value; break;
                case Attribute.Vit: vitality = value; break;
                case Attribute.Spt: spirit = value; break;
            }
        }
    }
}