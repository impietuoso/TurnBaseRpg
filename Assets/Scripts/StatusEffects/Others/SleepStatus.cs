using UnityEngine;

[System.Serializable]
public class SleepStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnDefend += OnDefend;
        target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnDefend -= OnDefend;
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is SleepStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnDefend(CombatArgs args) {
        if (args.damage > 0) {
            Debug.Log(args.target.characterName + " woke up from damage!");
            args.target.StatusEffectList.Remove(source);
        }
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}