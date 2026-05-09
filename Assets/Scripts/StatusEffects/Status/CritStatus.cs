[System.Serializable]
public class CritStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public int bonusValue = 30; // Additive bonus for critical chance

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
        if (other is CritStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnAttack(CombatArgs args) {
        args.criticalChance += bonusValue;
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}