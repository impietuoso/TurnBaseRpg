[System.Serializable]
public class ArmorStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public float multiplier = 1.3f;

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.derivedStats.armor.AddBonus(this, multiplier);
        //target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.derivedStats.armor.RemoveBonus(this);
        //target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is ArmorStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}