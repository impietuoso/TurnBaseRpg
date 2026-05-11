[System.Serializable]
public class Silence : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        //target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        //target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is SpeedStatus otherStatus) {
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
