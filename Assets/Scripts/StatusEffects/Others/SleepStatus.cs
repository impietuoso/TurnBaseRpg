using UnityEngine;

[System.Serializable]
public class SleepStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnStartTurn += OnTurnStart;
        target.OnDefend += OnDefend;
        target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnStartTurn -= OnTurnStart;
        target.OnDefend -= OnDefend;
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is SleepStatus otherStatus) {
            this.duration = otherStatus.duration;
        }
    }

    private void OnTurnStart(Character target) {
        if (CombatManager.instance.currentCharacter == target) {
            Debug.Log(target.characterName + " is sleeping and skips turn!");
            CombatManager.instance.SkipTurn();
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