using System;
using System.Collections;
using Drafts;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Core")]
public class Core : Equipment {
    [SerializeField] private Element element;
    [SerializeField, TwoColumns] private CoreStats stats;
    [SerializeReference, TypeInstance] private ICombatEffect[] effects;
    [SerializeReference, TypeInstance] private IPassive[] passives;

    public override EquipSlot EquipSlot => EquipSlot.Core;
    public Element Element => element;
    public ICombatEffect[] Effects => effects;
    public IPassive[] Passives => passives;

    public float GetChargeTime(Character user) => user.Stats[Stat.Speed] / 10f;
    public IEnumerator Execute(ActionArgs args) => throw new NotImplementedException();

    [Obsolete] public Skill basicAttack;
    public override int this[Stat stat] => stats[stat];
}

[Serializable]
public class CoreStats : IStats {
    [field: SerializeField, Range(0, 100)] public int Damage { get; private set; }
    [field: SerializeField, Range(0, 100)] public int Hit { get; private set; }
    [field: SerializeField, Range(0, 100)] public int Evade { get; private set; }
    [field: SerializeField, Range(0, 100)] public int CritChance { get; private set; }
    [field: SerializeField, Range(0, 100)] public int CritDamage { get; private set; }
    [field: SerializeField, Range(0, 100)] public int CastSpeed { get; private set; }

    public int this[Stat stat] => stat switch {
        Stat.Damage => Damage,
        Stat.Hit => Hit,
        Stat.Evade => Evade,
        Stat.CritChance => CritChance,
        Stat.CritDamage => CritDamage,
        Stat.CastSpeed => CastSpeed,
        _ => 0
    };
}