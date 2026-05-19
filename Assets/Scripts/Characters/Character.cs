using UnityEngine;
using System;
using System.Collections.Generic;
using DefaultNamespace;
using TricksAndTreatsOrThreats;
using TricksAndTreatsOrThreats.Behaviour;

public partial class Character {
    public CombatController CombatController { get; private set; }

    [Header("Info")]
    private Creature creature;
    public bool isAlly;
    [Obsolete] public List<Skill> basicAttack = new ();
    public StatusEffectList StatusEffectList;

    public Element Element { get; private set; }

    public Action<CombatArgs> OnDefend;
    public Action<CombatArgs> OnAttack;
    public Action<CombatArgs> OnResolveDefend;
    public Action<CombatArgs> OnResolveAttack;

    public Creature Creature => creature;
    public DataMap DataMap = new ();

    public void Spawn(CombatController cc, Creature creature, bool allyTeam) {
        CombatController = cc;
        this.creature = creature;
        name = this.creature.DisplayName;
        isAlly = allyTeam;
        SpriteRenderer.sprite = Creature.Race.Sprite;

        if (!allyTeam) name += " (wild)";
        this.creature.Equips.OnChanged += UpdateEquipPassives;

        InitializeStats();
        SubscribePassives();
        SubscribePassives();
    }

    public void Despawn() {
        creature.Equips.OnChanged -= UpdateEquipPassives;

        UnsubscribePassives();
        Destroy(gameObject);
    }

    private void UpdateEquipPassives(ListChangedArgs<Equipment> e) {
        if (e.action != ListAction.Replace) throw new Exception("Replace expected");
        if (e.oldItem) e.oldItem.Passive?.Subscribe(this);
        if (e.newItem) e.newItem.Passive?.Subscribe(this);
    }

    private void SubscribePassives() {
        foreach (var equip in creature.Equips)
            if (equip)
                equip.Passive?.Subscribe(this);
        foreach (var passive in creature.Passives)
            if (passive)
                passive.Subscribe(this);
    }

    private void UnsubscribePassives() {
        foreach (var equip in creature.Equips)
            if (equip)
                equip.Passive?.Unsubscribe(this);
        foreach (var passive in creature.Passives)
            if (passive)
                passive.Unsubscribe(this);
    }
}