using System;
using System.Collections;
using System.Linq;
[Serializable]
public class OnDealDamage : IPassiveSkill {
    public Element element;
    public Skill selfUseSkill;
    public bool castOnSelf;

    public void Subscribe(Character character) {
        character.OnResolveAttack += ConsequencesOfSeuActs;
    }

    public void Unsubscribe(Character character) {
        character.OnResolveAttack -= ConsequencesOfSeuActs;
    }

    public void ConsequencesOfSeuActs(CombatArgs args) {
        if (element && args.skillElement != element) return;
        if (args.result.deltaHp >= 0) return;
        if (args.stopReactionAttacks) return;
        
        
        
        if (CombatManager.instance.combatEvents.Any(e => e is OnDealDamageEvent
                otd && otd.thisEvent == this && otd.args.target == args.target)) return;
        
        var combatEvent = new OnDealDamageEvent(args, this);
        CombatManager.instance.combatEvents.Enqueue(combatEvent);
    }
}

public class OnDealDamageEvent : ICombatPhase {
    public CombatArgs args;
    public OnDealDamage thisEvent;

    public OnDealDamageEvent(CombatArgs args, OnDealDamage thisEvent) {
        this.args = args;
        this.thisEvent = thisEvent;
    }

    public IEnumerator Execute(CombatManager cm) {
        yield return thisEvent.selfUseSkill.UseSkill(args.user, thisEvent.castOnSelf ? args.user : args.target, CombatManager.instance);
    }
}