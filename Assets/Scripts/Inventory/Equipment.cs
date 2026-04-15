using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item/Equipment", fileName = "New Equipment")]
public class Equipment : Item {
    public EquipmentType equipmentType;
    public Tag categoryTag;
    public DerivedStatsBase bonusValue;
    public Skill equipmentSkill;

    public string BonusText() {
        var resume = "<b>" + displayName + ":</b>\n";
        if (bonusValue.damage != 0) resume += $"Damage: {bonusValue.damage}\n";
        if (bonusValue.health != 0) resume += $"Health: {bonusValue.health}\n";
        if (bonusValue.mana != 0) resume += $"Mana: {bonusValue.mana}\n";
        if (bonusValue.shield != 0) resume += $"Shield: {bonusValue.shield}\n";
        if (bonusValue.speed != 0) resume += $"Speed: {bonusValue.speed}\n";
        if (bonusValue.armor != 0) resume += $"Armor: {bonusValue.armor}\n";
        if (bonusValue.resistance != 0) resume += $"Resistance: {bonusValue.resistance}\n";
        if (bonusValue.evade != 0) resume += $"Evade: {bonusValue.evade}\n";
        return resume;
    }
}