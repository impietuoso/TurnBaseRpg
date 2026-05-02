using UnityEngine;

[System.Serializable]
public class BleedingStatus: Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public int damage = 10;
    
    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnAttack += OnAttack;
        target.OnEndTurn += OnTurnEnd; // To tick duration
    }

    public override void Remove(Character target) {
        target.OnAttack -= OnAttack;
        target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is BurningStatus otherStatus) {
            this.duration = otherStatus.duration;
        }
    }
    
    private void OnAttack(CombatArgs args) {
        CombatArgs newArgs = new();
        newArgs.source = this;
        newArgs.unavoidable = true;
        newArgs.ignoreShield = true;
        newArgs.ignoreArmor = true;
        newArgs.stopReactionAttacks = true;
        newArgs.target = args.user;
        newArgs.damage = damage;
        newArgs.Resolve();
        Debug.Log(newArgs.target.characterName + " takes " + damage + " bleed damage.");
    }

    private void OnTurnEnd(Character target) {
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}
