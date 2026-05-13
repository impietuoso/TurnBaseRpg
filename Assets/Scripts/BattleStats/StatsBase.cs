using System;
using UnityEngine;

public enum Stat {
    MaxHealth,
    MaxShield,
    MaxMana,

    Speed,
    Hit,
    Evade,
    Armor,
    Resistance,

    Damage,
    CritChance,
    CritDamage,
    CastSpeed,
}

public interface IStats {
    int this[Stat stat] { get; }
}

[Serializable]
public class StatsBase : IStats {
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Shield { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int Hit { get; private set; }
    [field: SerializeField] public int Evade { get; private set; }
    [field: SerializeField] public int Armor { get; private set; }
    [field: SerializeField] public int Resistance { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int CritChance { get; private set; }
    [field: SerializeField] public int CritDamage { get; private set; }
    [field: SerializeField] public int CastSpeed { get; private set; }

    public int this[Stat stat] => stat switch {
        Stat.MaxHealth => Health,
        Stat.MaxShield => Shield,
        Stat.MaxMana => Mana,
        Stat.Speed => Speed,
        Stat.Hit => Hit,
        Stat.Evade => Evade,
        Stat.Armor => Armor,
        Stat.Resistance => Resistance,
        Stat.Damage => Damage,
        Stat.CritChance => CritChance,
        Stat.CritDamage => CritDamage,
        Stat.CastSpeed => CastSpeed,
        _ => throw new NotImplementedException()
    };
}