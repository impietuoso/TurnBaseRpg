using System;
using System.Collections;
using System.Linq;
[Serializable]
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
        
        if (!args.actionArgs.Flags.Add(this)) return;
        
        var cc = args.user.CombatController;
        var newTarget = castOnSelf ? args.target : args.user;
        cc.StartCoroutine(counterSkill.animation.Play(counterSkill, args.user, newTarget));
    }
}