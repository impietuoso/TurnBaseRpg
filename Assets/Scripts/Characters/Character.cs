using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TricksAndTreatsOrThreats.Behaviour;

public partial class Character {
    public CombatController CombatController { get; private set; }

    [Header("Info")]
    private PartyMember member;
    public bool isAlly;
    public Element element;
    [Obsolete] public List<Skill> basicAttack = new ();
    public List<Skill> skills = new ();
    public StatusEffectList StatusEffectList;

    public Action<CombatArgs> OnDefend;
    public Action<CombatArgs> OnAttack;
    public Action<CombatArgs> OnResolveDefend;
    public Action<CombatArgs> OnResolveAttack;

    private List<IPassive> passives = new ();

    public PartyMember Member => member;
    public DataMap DataMap = new ();

    public void Spawn(CombatController cc, PartyMember memberOjb, bool allyTeam) {
        CombatController = cc;
        member = memberOjb;
        name = member.charName;
        isAlly = allyTeam;
        if (!allyTeam) name += " (wild)";

        element = member.element;
        skills = member.equipedSkills.Where(s => s && s.animation != null).ToList();


        InitializeStats();
        UpdateBehaviour();
        SubscribePassives();
    }

    public void Despawn() {
        UnsubscribePassives();
        Destroy(gameObject);
    }

    private void SubscribePassives() {
        foreach (var equip in member.equips)
            if (equip && equip.passive != null)
                equip.passive.Subscribe(this);
        foreach (var skill in member.equipedSkills)
            if (skill && skill.passiva != null)
                skill.passiva.Subscribe(this);
    }

    private void UnsubscribePassives() {
        foreach (var equip in member.equips)
            if (equip && equip.passive != null)
                equip.passive.Unsubscribe(this);
        foreach (var skill in member.equipedSkills)
            if (skill && skill.passiva != null)
                skill.passiva.Unsubscribe(this);
    }
}