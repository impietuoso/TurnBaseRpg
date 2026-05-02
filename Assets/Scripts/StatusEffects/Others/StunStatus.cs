using UnityEngine;

[System.Serializable]
public class StunStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (1);

    public override void Apply(Character target) {
        target.StatusEffectList.Remove(opposite);
        target.OnStartTurn += OnTurnStart;
        target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnStartTurn -= OnTurnStart;
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is SleepStatus otherStatus) {
            this.duration = otherStatus.duration;
        }
    }

    private void OnTurnStart(Character target) {
        if (CombatManager.instance.currentCharacter == target) {
            Debug.Log(target.characterName + " is stunned!");
            CombatManager.instance.TurnManager();
        }
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}
