using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Character{
    public Character(PartyMember member, string newTeam) {
        stats = member.usedStats;
        skills = member.equipedSkills.ToList();
        equipment = member.equips.ToList();
        characterName = member.charName;
        profession = member.profession;
        characterSprite = member.characterSprite;
        uiSprite = member.uiSprite;
        team = newTeam;
        element = member.element;
        derivedStats = new();
        UpdateCombatValues();
    }
    
    public Character() {
        
    }
    
    public string characterName;
    [Header("Base Stats")]
    [SerializeField]
    public BaseStats stats;
    public List<Equipment> equipment;

    [Header("Player Combat Info")]
    public DerivedStats derivedStats;

    [Header("Advancement Info")]
    [SerializeField]
    public int level = 1;
    public Profession profession;
    public List<Skill> basicAttack = new();
    public List<Skill> skills = new();
    public Sprite characterSprite;
    public Sprite uiSprite;
    public string team;
    public StatusEffectList StatusEffectList;
    public Element element;

    public Action<Character> OnStartTurn;
    public Action<Character> OnEndTurn;
    public Action<CombatArgs> OnDefend;
    public Action<CombatArgs> OnAttack;
    public Action<CombatArgs> OnResolveDefend;
    public Action<CombatArgs> OnResolveAttack;

    public Observable<float> actionPoints = new();

    private void UpdateCombatValues() {
        StatusEffectList = new StatusEffectList(this);
        derivedStats.CalculateDeviredStats(stats, profession, equipment, level);
        foreach (var equip in equipment) {
            if (equip == null || equip.equipmentSkill == null)
                continue;

            if (equip is Weapon w) {
                basicAttack.Add(w.basicAttack);
            } else {
                if (!skills.Contains(equip.equipmentSkill)) {
                    skills.Add(equip.equipmentSkill);
                }
            }
        }

        if (basicAttack.Count == 0) {
            basicAttack.Add(profession.basicAttack);
        }
    }
}