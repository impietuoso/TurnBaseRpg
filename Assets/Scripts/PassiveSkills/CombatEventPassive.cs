using System;
using Drafts;
using UnityEngine;
using UnityEngine.Scripting;

public enum Retarget {
    User,
    Target,
}

[Preserve, Serializable]
public abstract class CombatArgsPassive : IPassiveSkill {
    [SerializeReference, TypeInstance] private ICombatArgsFilter condition;
    [SerializeReference, TypeInstance] private Retarget retarget = Retarget.User;
    [SerializeReference, TypeInstance] private ISkillEffect[] effects;

    protected abstract void SubscribeCondition(Character character);
    protected abstract void UnsubscribeCondition(Character character);

    public void Subscribe(Character character) => SubscribeCondition(character);
    public void Unsubscribe(Character character) => UnsubscribeCondition(character);

    protected void Trigger(CombatArgs args) {
        if (!condition.Match(args)) return;

        var triggerArgs = new CombatArgs();
        triggerArgs.actionArgs = args.actionArgs;
        triggerArgs.source = this;
        triggerArgs.user = args.user;
        triggerArgs.target = retarget == Retarget.User ? args.user : args.target;

        foreach (var e in effects) e.Prepare(triggerArgs);
        triggerArgs.Resolve();
    }
}

[Preserve, Serializable]
public class OnAttack : CombatArgsPassive {
    protected override void SubscribeCondition(Character character) => character.OnAttack += Trigger;
    protected override void UnsubscribeCondition(Character character) => character.OnAttack -= Trigger;
}

[Preserve, Serializable]
public class OnDefend : CombatArgsPassive {
    protected override void SubscribeCondition(Character character) => character.OnDefend += Trigger;
    protected override void UnsubscribeCondition(Character character) => character.OnDefend -= Trigger;
}

[Preserve, Serializable]
public class OnResolveAttack : CombatArgsPassive {
    protected override void SubscribeCondition(Character character) => character.OnResolveAttack += Trigger;
    protected override void UnsubscribeCondition(Character character) => character.OnResolveAttack -= Trigger;
}

[Preserve, Serializable]
public class OnResolveDefend : CombatArgsPassive {
    protected override void SubscribeCondition(Character character) => character.OnResolveDefend += Trigger;
    protected override void UnsubscribeCondition(Character character) => character.OnResolveDefend -= Trigger;
}