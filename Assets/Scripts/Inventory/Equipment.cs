using Drafts;
using UnityEngine;

public enum EquipSlot {
    None,
    Infusion,
    Coat,
    Core,
    Misc,
}

[CreateAssetMenu(menuName = "Scriptable/Item/Equipment", fileName = "New Equipment")]
public class Equipment : InventoryItem, IAttributes, IStats {
    [SerializeField] private Tag category;
    [SerializeReference, TypeInstance, Separator] public IPassive passive;

    public virtual EquipSlot EquipSlot { get; }
    public Tag Category => category;

    public string BonusText() {
        var resume = "<b>" + displayName + ":</b>\n";
        foreach (var attr in Attributes.All) {
            var v = this[attr];
            if (v > 0) resume += $"{attr}: {v}\n";
        }
        foreach (var stat in Stats.All) {
            var v = this[stat];
            if (v > 0) resume += $"{stat}: {v}\n";
        }
        return resume;
    }

    public virtual int this[Attribute attribute] => 0;
    public virtual int this[Stat stat] => 0;
}