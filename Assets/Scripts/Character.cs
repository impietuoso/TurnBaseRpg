using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Character
{
    public void Initialize(PartyMember member, string newTeam)
    {
        stats = member.usedStats;
        skills = member.equipedSkills.Where(s => s && s.passiva == null).ToList();
        equipment = member.equips.ToList();
        characterName = member.charName;
        profession = member.profession;
        characterSprite = member.characterSprite;
        uiSprite = member.uiSprite;
        team = newTeam;
        element = member.element;
        this.member = member;
        derivedStats = new();

        passives = member.equips.Where(e => e && e.passiva != null).Select(e => e.passiva)
            .Concat(member.equipedSkills.Where(s => s && s.passiva != null).Select(e => e.passiva))
            .ToList();

        foreach (var Skill in member.equipedSkills)
        {
            if (Skill && Skill.passiva != null)
            {
                passives.Add(Skill.passiva);
            }
        }

        UpdateCombatValues();
        UpdateBehaviour();
    }

    public Character() { }

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
    public PartyMember member;

    public Action<Character> OnSetup;
    public Action<Character> OnStartTurn;
    public Action<Character> OnEndTurn;
    public Action<CombatArgs> OnDefend;
    public Action<CombatArgs> OnAttack;
    public Action<CombatArgs> OnResolveDefend;
    public Action<CombatArgs> OnResolveAttack;

    public List<IPassiveSkill> passives = new();

    public Observable<float> actionPoints = new();

    public void SubscribePassives()
    {
        foreach (var passive in passives)
        {
            passive.Subscribe(this);
        }
    }

    public void UnsubscribePassives()
    {
        foreach (var passive in passives)
        {
            passive.Unsubscribe(this);
        }
    }

    private void UpdateCombatValues()
    {
        StatusEffectList = new StatusEffectList(this);
        derivedStats.CalculateDeviredStats(stats, profession, equipment, level);
        foreach (var equip in equipment)
        {
            if (equip == null || equip.equipmentSkill == null)
                continue;

            if (equip is Weapon w)
            {
                basicAttack.Add(w.basicAttack);
            }
            else
            {
                if (!skills.Contains(equip.equipmentSkill))
                {
                    skills.Add(equip.equipmentSkill);
                }
            }
        }

        if (basicAttack.Count == 0)
        {
            basicAttack.Add(profession.basicAttack);
        }
    }
}