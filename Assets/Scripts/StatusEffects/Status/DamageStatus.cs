using UnityEngine;

[System.Serializable]
public class DamageStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public float multiplier = 1.3f;

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnAttack += OnAttack;
        target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnAttack -= OnAttack;
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is DamageStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }
    
    private void OnAttack(CombatArgs args) {
        args.damage = (int)(args.damage * multiplier);
    }
    
    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}