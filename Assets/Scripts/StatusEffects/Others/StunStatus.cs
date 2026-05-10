using UnityEngine;

[System.Serializable]
public class StunStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (1);

    public override void Apply(Character target) {
        target.StatusEffectList.Remove(opposite);
        target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is SleepStatus otherStatus) {
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
