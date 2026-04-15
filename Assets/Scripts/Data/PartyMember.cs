using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Party Member", menuName = "Scriptable/PartyMember")]
public class PartyMember : ScriptableObject {
    public string charName;
    public int level;
    public BaseStats usedStats;
    public Profession profession;
    [ShowEquipmentTypeAtribute] public ObservableList<Equipment> equips;
    public ObservableList<Skill> equipedSkills;
    public ObservableList<Skill> learnedSkills;
    public Sprite characterSprite;
    public Sprite uiSprite;

    public int GetUnusedPoints() {
        var maxPoints = (level - 1) * 2 + 25;
        var usedPoints = 0;
        foreach (var stat in usedStats) {
            usedPoints += stat;
        }
        return maxPoints - usedPoints;
    }
}