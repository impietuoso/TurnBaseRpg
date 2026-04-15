using System;
using UnityEngine;

[Serializable]
public class DerivedStatsBase {
    [field:SerializeField] public int damage { get; private set; }
    [field:SerializeField] public int health { get; private set; }
    [field:SerializeField] public int shield { get; private set; }
    [field:SerializeField] public int mana { get; private set; }
    [field:SerializeField] public int speed { get; private set; }
    [field:SerializeField] public int armor { get; private set; }
    [field:SerializeField] public int resistance { get; private set; }
    [field:SerializeField] public int evade { get; private set; }
}
