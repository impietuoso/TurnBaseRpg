using System;
using TricksAndTreatsOrThreats.Behaviour;

[Serializable]
public class OnTakeDamage : IPassive {
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
        var aArgs = new ActionArgs(args.skill, args.user, newTarget);
        cc.StartCoroutine(counterSkill.animation.Play(counterSkill, aArgs));
    }
}