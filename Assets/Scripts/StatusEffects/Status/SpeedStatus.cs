using UnityEngine;

[System.Serializable]
public class SpeedStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public float multiplier = 1.3f;

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.derivedStats.speed.AddBonus(this, multiplier);
        target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.derivedStats.speed.RemoveBonus(this);
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is SpeedStatus otherStatus) {
            this.duration = otherStatus.duration;
        }
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}