using System;
using System.Collections.Generic;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatArgs {
    public ActionArgs actionArgs;
    public object source;
    public Character user;
    public Character target;
    public Skill skill;
    public bool unavoidable;
    public bool ignoreShield;
    public bool ignoreArmor;
    public bool stopReactionAttacks;
    public int damage;
    public int heal;
    public int mana;
    public int manaHeal;
    public int shield;
    public int criticalChance;
    public int hitChance;
    public Element skillElement;
    public CombatResult result;
    public List<StatusSO> statusEffects = new();
    public Action<CombatArgs> OnResolve;
    
    public void Resolve() {
        if (result != null) return;
        user?.OnAttack?.Invoke(this);
        target?.OnDefend?.Invoke(this);
        float chance = Random.Range(0f, 100f);

        var miss = !unavoidable && chance > hitChance;

        int previousHp = target?.derivedStats.health.currentValue ?? 0;
        int previousMp = target?.derivedStats.mana.currentValue ?? 0;
        int previousShield = target?.derivedStats.shield.currentValue ?? 0;
        float currentCriticalChance = Random.Range(0f, 100f);

        if (!ignoreArmor) {
            damage = Mathf.Max(damage - (target?.derivedStats.armor.currentValue ?? 0), 0);
        }

        if (!miss) {
            if (currentCriticalChance <= criticalChance) damage *= 2;
            if (!ignoreShield) {
                var currentShield = target?.derivedStats.shield.currentValue ?? 0;
                target?.derivedStats.shield.AddClampedBaseValue(-damage);
                damage = Mathf.Max(damage - currentShield, 0);
            }
        } else {
            damage = 0;
        }

        if (skillElement) {
            if (target?.element.weak.Contains(skillElement)??false)
                damage = (int)(damage * 1.2f);
            else if (skillElement.weak.Contains(target?.element))
                damage = (int)(damage * 0.8f);
        }

        target?.derivedStats.health.AddClampedBaseValue(heal - damage);
        target?.derivedStats.mana.AddClampedBaseValue(mana);
        target?.derivedStats.shield.AddClampedBaseValue(shield);

        user?.derivedStats.mana.AddClampedBaseValue(manaHeal);

        var resist = false;

        foreach (var effect in statusEffects) {
            if (effect.statusType != StatusType.debuff) target?.StatusEffectList.Apply(effect);
            else {
                if (!actionArgs.Flags.Add(effect)) {
                    continue;
                }
                var applyChance = Random.Range(0, 100);
                if (applyChance <= 100 - target?.derivedStats.resistance.currentValue) {
                    target.StatusEffectList.Apply(effect);
                    resist = false;
                    Debug.Log(effect.status + " was add to queue " + target?.Member.charName + ".");
                } else
                    resist = true;
            }
        }

        result = new CombatResult();
        result.deltaShield = (target?.derivedStats.shield.currentValue ?? 0) - previousShield;
        result.deltaHp = (target?.derivedStats.health.currentValue ?? 0) - previousHp;
        result.isCrit = currentCriticalChance <= criticalChance && !miss;
        result.isFatal = target?.derivedStats.health.currentValue == 0 && result.deltaHp < 0;
        result.isRevive = previousHp == 0 && target?.derivedStats.health.currentValue > 0;
        result.miss = miss;
        result.resistStatus = resist;
        result.deltaMp = (target?.derivedStats.mana.currentValue ?? 0) - previousMp;

        user?.OnResolveAttack?.Invoke(this);
        target?.OnResolveDefend?.Invoke(this);

        OnResolve?.Invoke(this);

        if (result.miss) Debug.Log("Miss");
    }
}

public class CombatResult {
    public int deltaHp;
    public int deltaMp;
    public int deltaShield;
    public bool isCrit;
    public bool isFatal;
    public bool isRevive;
    public bool miss;
    public bool resistStatus;
}
