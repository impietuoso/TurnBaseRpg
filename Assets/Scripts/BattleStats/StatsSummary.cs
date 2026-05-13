using System;

[Serializable]
public class StatsSummary : Stats {
    public BonusStat MaxHealth => this[Stat.MaxHealth];
    public BonusStat MaxShield => this[Stat.MaxShield];
    public BonusStat MaxMana => this[Stat.MaxMana];

    public BonusStat Speed => this[Stat.Speed];
    public BonusStat Hit => this[Stat.Hit];
    public BonusStat Evade => this[Stat.Evade];
    public BonusStat Armor => this[Stat.Armor];
    public BonusStat Resistance => this[Stat.Resistance];

    public BonusStat Damage => this[Stat.Damage];
    public BonusStat CritChance => this[Stat.CritChance];
    public BonusStat CritDamage => this[Stat.CritDamage];
    public BonusStat CastSpeed => this[Stat.CastSpeed];
}