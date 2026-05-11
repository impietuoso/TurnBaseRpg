using System;
using TricksAndTreatsOrThreats.Behaviour;

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
        if (!args.actionArgs.Flags.Add(this)) return;
        
        var cc = args.user.CombatController;
        var newTarget = castOnSelf ? args.user : args.target;
        var aArgs = new ActionArgs(args.skill, args.user, newTarget);
        cc.StartCoroutine(selfUseSkill.animation.Play(selfUseSkill, aArgs));
    }
}