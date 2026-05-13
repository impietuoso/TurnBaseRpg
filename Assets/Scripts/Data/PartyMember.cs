using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Scriptable/PartyMember")]
public class PartyMember : ScriptableObject, IAttributes, IStats {
    public string charName;
    public int level;
    public Attributes usedStats;
    public Profession profession;
    public Element element;
    [EquipArray] public ObservableList<Equipment> equips;
    public ObservableList<Skill> equipedSkills;
    public ObservableList<Skill> learnedSkills;
    public Sprite characterSprite;
    public Sprite uiSprite;

    public int GetUnusedPoints() => level * 2 - usedStats.Sum();

    public IEnumerable<IStats> GetStatsBonus() {
        foreach (var equip in equips)
            if (equip is IStats s)
                yield return s;
    }

    public int this[Attribute a] => usedStats[a] + profession.InitialStats[a] + equips.Sum(i => i[a]);
    public int this[Stat s] => equips.Sum(i => i[s]);
}