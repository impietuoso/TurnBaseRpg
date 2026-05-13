using System;
using Drafts;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Infusion")]
public class Infusion : Equipment {
    [SerializeField] private Element element;
    [SerializeField, Separator, TwoColumns] private InfusionStats stats;
    [SerializeReference, TypeInstance] private ICombatEffect effect;

    // [field: SerializeField] public StatScale Stat { get; private set; }
    // [field: SerializeField] public float Variation { get; private set; } = 0.15f;
    // [field: SerializeField] public float RangeAdd { get; private set; }

    public override EquipSlot EquipSlot => EquipSlot.Infusion;
    public Element Element => element;
    public ICombatEffect Effect => effect;
    
    public override int this[Stat stat] => stats[stat];
}

[Serializable, SingleLine]
public class InfusionStats : IStats {
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