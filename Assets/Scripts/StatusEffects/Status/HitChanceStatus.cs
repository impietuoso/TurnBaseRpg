[System.Serializable]
public class HitChanceStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public int hitBonus = 30;

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnAttack += OnAttack;
        //target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnAttack -= OnAttack;
        //target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is HitChanceStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnAttack(CombatArgs args) {
        args.hitChance += hitBonus;
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}