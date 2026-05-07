using System.Collections;
using System.Linq;

public class OnTakeDamage : IPassiveSkill {
    public Element element;
    public Skill counterSkill;
    public bool castOnSelf;

    public void Subscribe(Character character) {
        character.OnResolveDefend += ConsequencesOfSeuActs;
    }

    public void Unsubscribe(Character character) {
        character.OnResolveDefend -= ConsequencesOfSeuActs;
    }

    public void ConsequencesOfSeuActs(CombatArgs args) {
        if (element && args.skillElement != element) return;
        if (args.result.deltaHp >= 0) return;
        if (args.stopReactionAttacks) return;
        
        
        
        if (CombatManager.instance.combatEvents.Any(e => e is OnTakeDamageEvent
                otd && otd.thisEvent == this && otd.args.target == args.target)) return;
        
        var combatEvent = new OnTakeDamageEvent(args, this);
        CombatManager.instance.combatEvents.Enqueue(combatEvent);
    }
}
public class OnTakeDamageEvent : ICombatPhase {
    public CombatArgs args;
    public OnTakeDamage thisEvent;

    public OnTakeDamageEvent(CombatArgs args, OnTakeDamage thisEvent) {
        this.args = args;
        this.thisEvent = thisEvent;
    }

    public IEnumerator Execute(CombatManager cm) {
        yield return thisEvent.counterSkill.UseSkill(args.target, thisEvent.castOnSelf ? args.target : args.user, CombatManager.instance);
    }
}