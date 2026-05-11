using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TricksAndTreatsOrThreats.Behaviour;

public partial class Character {
    public CombatController CombatController { get; private set; }

    [Header("Stats")]
    public int level = 1;
    public BaseStats stats;
    public DerivedStats derivedStats;

    [Header("Info")]
    private PartyMember member;
    public string team;
    public Profession profession;
    public Element element;
    public List<Equipment> equipment;
    public List<Skill> basicAttack = new ();
    public List<Skill> skills = new ();
    public StatusEffectList StatusEffectList;

    public Action<CombatArgs> OnDefend;
    public Action<CombatArgs> OnAttack;
    public Action<CombatArgs> OnResolveDefend;
    public Action<CombatArgs> OnResolveAttack;

    private List<IPassiveSkill> passives = new ();

    public PartyMember Member => member;
    public Observable<float> actionPoints = new ();
    public DataMap DataMap = new ();

    public void Initialize(CombatController cc, PartyMember member, string newTeam) {
        CombatController = cc;
        this.member = member;
        name = $"{member.charName} ({newTeam})";
        team = newTeam;

        stats = member.usedStats;
        skills = member.equipedSkills.Where(s => s && s.passiva == null).ToList();
        equipment = member.equips.ToList();
        profession = member.profession;
        element = member.element;
        derivedStats = new ();

        passives = member.equips.Where(e => e && e.passiva != null).Select(e => e.passiva)
            .Concat(member.equipedSkills.Where(s => s && s.passiva != null).Select(e => e.passiva))
            .ToList();

        UpdateCombatValues();
        UpdateBehaviour();
        SubscribePassives();
    }

    private void SubscribePassives() {
        foreach (var passive in passives)
            passive.Subscribe(this);
    }

    private void UnsubscribePassives() {
        foreach (var passive in passives)
            passive.Unsubscribe(this);
    }

    private void UpdateCombatValues() {
        StatusEffectList = new StatusEffectList(this);
        derivedStats.CalculateDeviredStats(stats, profession, equipment, level);
        foreach (var equip in equipment) {
            if (!equip || !equip.equipmentSkill) continue;

            if (equip is Weapon w)
                basicAttack.Add(w.basicAttack);
            else {
                if (!skills.Contains(equip.equipmentSkill))
                    skills.Add(equip.equipmentSkill);
            }
        }

        if (basicAttack.Count == 0)
            basicAttack.Add(profession.basicAttack);
    }
}