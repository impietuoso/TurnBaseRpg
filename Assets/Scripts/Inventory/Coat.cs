using System;
using Drafts;
using UnityEngine;

[Obsolete]
[CreateAssetMenu(menuName = "Scriptable/Item/Coat")]
public class Coat : Equipment {
    [SerializeField] private Element element;
    [SerializeField, TwoColumns] private CoatStats stats;
    [SerializeReference, TypeInstance, Separator] private IPassive passive;

    public override EquipSlot EquipSlot => EquipSlot.Core;
    public Element Element => element;
    public override IPassive Passive => passive;

}

[Serializable]
public class CoatStats : IStats {
    public int this[Stat stat] => throw new NotImplementedException();
}