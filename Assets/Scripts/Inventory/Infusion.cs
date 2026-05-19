using System;
using Drafts;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Infusion")]
public class Infusion : Equipment {
    [SerializeField] private Element element;
    [SerializeField] private InfusionStats stats;
    [SerializeReference, TypeInstance, Separator] private ICombatEffect effect;
    [SerializeReference, TypeInstance, Separator] private IPassive passive;

    public override EquipSlot EquipSlot => EquipSlot.Infusion;
    public Element Element => element;
    public ICombatEffect Effect => effect;
    public override IPassive Passive => passive;

    public override int this[Stat stat] => stats[stat];
}

[Serializable]
public class InfusionStats : IStats, ITwoColumnsDrawer {
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int Hit { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int CastSpeed { get; private set; }
    [field: SerializeField] public int CritChance { get; private set; }
    [field: SerializeField] public int CritDamage { get; private set; }

    public int this[Stat stat] => stat switch {
        Stat.Damage => Damage,
        Stat.Hit => Hit,
        Stat.CritChance => CritChance,
        Stat.CritDamage => CritDamage,
        Stat.Speed => Speed,
        Stat.CastSpeed => CastSpeed,
        _ => 0
    };
}