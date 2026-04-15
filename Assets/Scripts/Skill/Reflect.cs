using System.Collections;
using UnityEngine;

[System.Serializable]
public class ReflectEffect : Status {
    [Range(0,10)]
    public float reflectPercentage;
    public bool protectFromDamage;
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnEndTurn += OnTurnEnd;
        target.OnDefend += OnTakeDamage;
    }

    public override void Remove(Character target) {
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is ReflectEffect otherStatus) {
            duration = otherStatus.duration;
        }
    }

    private void OnTakeDamage(CombatArgs args) {
        if (args.user == null || args.stopReactionAttacks) return;
        var damageReflected = (int)(args.damage * reflectPercentage);
        args.damage -= protectFromDamage ? damageReflected : 0;
        
        CombatArgs newArgs = new();
        newArgs.source = this;
        newArgs.unavoidable = true;
        newArgs.target = args.user;
        newArgs.damage = damageReflected;
        newArgs.stopReactionAttacks = true;
        CombatManager.instance.combatEvents.Enqueue(ReflectDamage(newArgs));
    }

    public IEnumerator ReflectDamage (CombatArgs args) {
        yield return null;
        args.Resolve();
        Debug.Log(args.target.characterName + " takes " + args.damage + " reflect damage.");
    }
    
    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}
