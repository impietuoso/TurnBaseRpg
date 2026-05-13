using System;
using System.Collections;
using Drafts;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Coat")]
public class Coat : Equipment {
    [SerializeField] private Element element;
    [SerializeField, TwoColumns] private CoatStats stats;
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
public class CoatStats : IStats {
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