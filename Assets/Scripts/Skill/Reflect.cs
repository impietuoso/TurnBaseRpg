using System.Collections;
using UnityEngine;

[System.Serializable]
public class ReflectEffect : Status {
    [Range(0, 10)]
    public float reflectPercentage;
    public bool protectFromDamage;
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        //target.OnEndTurn += OnTurnEnd;
        target.OnDefend += OnTakeDamage;
    }

    public override void Remove(Character target) {
        //target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is ReflectEffect otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnTakeDamage(CombatArgs args) {
        if (!args.user || args.stopReactionAttacks) return;
        var damageReflected = (int)(args.damage * reflectPercentage);
        if (protectFromDamage) args.damage = 0;

        var chain = args.Chain(this, args.user);
        chain.damage = damageReflected;
        chain.unavoidable = true;

        args.CombatCoroutine(ReflectDamage(chain));
    }

    public IEnumerator ReflectDamage(CombatArgs args) {
        yield return new WaitForSeconds(0.5f);
        args.Resolve();
        Debug.Log(args.target.Creature.DisplayName + " takes " + args.damage + " reflect damage.");
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}