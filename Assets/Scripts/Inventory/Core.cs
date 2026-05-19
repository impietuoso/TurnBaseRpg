using System;
using Drafts;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Core")]
public class Core : Equipment {
    [SerializeField] private Element element;
    [SerializeField] private CoreStats stats;
    [SerializeReference, TypeInstance, Separator] private IPassive passive;

    public override EquipSlot EquipSlot => EquipSlot.Core;
    public Element Element => stats.Element;
    public override IPassive Passive => passive;

    public override int this[Stat stat] => stats[stat];
}

[Serializable]
public class CoreStats : IStats, ITwoColumnsDrawer {
    [field: SerializeField] public Element Element { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int Armor { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int CastSpeed { get; private set; }

    public int this[Stat stat] => stat switch {
        Stat.MaxHealth => MaxHealth,
        Stat.MaxMana => MaxMana,
        Stat.Armor => Armor,
        Stat.Speed => Speed,
        Stat.CastSpeed => CastSpeed,
        _ => 0
    };
}