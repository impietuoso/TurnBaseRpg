using UnityEngine;

[System.Serializable]
public class PoisonStatus : Status {
    public override Observable<int> DisplayValue => duration;
    public Observable<int> duration = new (3);
    public int damage = 10;
    public float healReduction = 0.6f; // 40% reduction means heal * 0.6
    public Element element;

    public override void Apply(Character target) {
        if (TryNullifyOpposite(target)) return;
        target.OnDefend += OnDefend;
        //target.OnEndTurn += OnTurnEnd;
    }

    public override void Remove(Character target) {
        target.OnDefend -= OnDefend;
        //target.OnEndTurn -= OnTurnEnd;
    }

    public override void Stack(Character target, Status other) {
        if (other is PoisonStatus otherStatus) {
            duration.Value = otherStatus.duration.Value;
        }
    }

    private void OnDefend(CombatArgs args) {
        if (args.heal > 0) {
            args.heal = (int)(args.heal * healReduction);
        }
    }

    private void OnTurnEnd(Character target) {
        CombatArgs args = new();
        args.element = element;
        args.source = this;
        args.unavoidable = true;
        args.ignoreShield = true;
        args.ignoreArmor = true;
        args.stopReactionAttacks = true;
        args.target = target;
        args.damage = damage;
        args.Resolve();
        Debug.Log(target.Member.charName + " takes " + damage + " poison damage.");
        
        duration.Value--;
        if (duration.Value <= 0) {
            target.StatusEffectList.Remove(source);
        }
    }
}