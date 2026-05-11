using UnityEngine;

[System.Serializable]
public class BurningStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public int damage = 10;
    public Element element;

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        //target.OnStartTurn += OnTurnStart;
        //target.OnEndTurn += OnTurnEnd; // To tick duration
    }

    public override void Remove(Character target) {
        //target.OnStartTurn -= OnTurnStart;
        //target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is BurningStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnTurnStart(Character target) {
        var args = new CombatArgs();
        args.skillElement = element;
        args.source = this;
        args.unavoidable = true;
        args.ignoreShield = true;
        args.ignoreArmor = true;
        args.stopReactionAttacks = true;
        args.target = target;
        args.damage = damage;
        args.Resolve();
        Debug.Log(target.Member.charName + " takes " + damage + " burning damage.");
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}