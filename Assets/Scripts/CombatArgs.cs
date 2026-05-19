using System;
using System.Collections;
using System.Collections.Generic;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;
using static Stat;
using Random = UnityEngine.Random;

public class CombatArgs {
    public ActionArgs actionArgs2;
    public object source;
    public Character user;
    public Character target;
    public Skill skill;

    public bool unavoidable;
    public bool ignoreShield;
    public bool ignoreArmor;
    public bool cannotCrit;
    public bool stopReactionAttacks;

    public int damage;
    public int heal;
    public int mana;
    public int shield;
    public int critChance;
    public int hitChance;

    public Element element;
    public CombatResult result;
    public List<StatusSO> statusEffects = new ();
    public Action<CombatArgs> OnResolve;
    public readonly List<ICombatEffect> Effects = new ();

    public CombatArgs Chain(object src, Character newTgt = null) => new () {
        source = src,
        target = newTgt ?? target,
        actionArgs2 = actionArgs2,
        skill = skill,
        user = user,
        stopReactionAttacks = true,
    };

    public void Resolve() {
        if (result != null) return;
        if (!target) return;

        // setup events
        user?.OnAttack?.Invoke(this);
        target.OnDefend?.Invoke(this);

        // rng
        hitChance += -target[Evade] + (user?[Hit] ?? 0);
        var hitRoll = Random.Range(0, 100);
        var critRoll = Random.Range(0, 100);
        var hit = unavoidable || hitRoll < hitChance;
        var crit = hit && !cannotCrit && user && critRoll < critChance;

        // result info
        result = new CombatResult {
            Miss = !hit,
            Crit = crit,
        };

        // armor reduction
        if (!ignoreArmor) damage = Mathf.Max(damage - target[Armor], 0);
        
        // damage stat
        if (user) damage = (int)(damage * (user[Damage] + 100 / 100f));

        // crit damage
        if (crit) {
            var critDmg = user[CritDamage] / 100f;
            damage = (int)(damage * critDmg);
        }
        
        if (!hit) damage = 0;

        // shield delta
        if (hit && !ignoreShield) {
            result.Shield = target.Shield.Add(shield - damage);
            if (result.Shield.Delta < 0) damage += result.Shield.Delta;
        } else
            result.Shield = new (target.Shield.Current);

        // element bonus
        if (hit && element) {
            if (target.Element.weak.Contains(element))
                damage = (int)(damage * 1.2f);
            else if (element.weak.Contains(target.Element))
                damage = (int)(damage * 0.8f);
        }

        // delta hp & mp
        result.Health = target.Health.Add(heal - damage);
        result.Mana = target.Mana.Add(mana);

        // apply status effect
        foreach (var effect in statusEffects) {
            if (effect.statusType != StatusType.debuff)
                target.StatusEffectList.Apply(effect);
            else {
                if (!actionArgs2.Flags.Add(effect)) continue;
                var applyChance = Random.Range(0, 100);
                if (applyChance <= 100 - target[Resistance]) {
                    target.StatusEffectList.Apply(effect);
                    Debug.Log($"{effect.status} applied to {target.Creature.DisplayName}.");
                } else
                    result.ResistStatus = true;
            }
        }

        // resolve events
        user?.OnResolveAttack?.Invoke(this);
        target.OnResolveDefend?.Invoke(this);
        OnResolve?.Invoke(this);

        if (result.Miss) Debug.Log("Miss");
    }

    public Coroutine CombatCoroutine(IEnumerator routine) => target.CombatController.StartCoroutine(routine);
}

public class CombatResult {
    public ResourceStat.Result Health;
    public ResourceStat.Result Mana;
    public ResourceStat.Result Shield;
    public bool Crit;
    public bool Miss;
    public bool ResistStatus;
    public bool IsFatal => Health.Fatal;
    public bool IsRevive => Health.Revive;
}