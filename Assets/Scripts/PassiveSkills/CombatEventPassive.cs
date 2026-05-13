using System;
using Drafts;
using UnityEngine;
using UnityEngine.Scripting;

public enum Retarget {
    User,
    Target,
}

[Preserve, Serializable]
public abstract class CombatArgsPassive : IPassive {
    [SerializeReference, TypeInstance] private ICombatArgsFilter condition;
    [SerializeField] private Retarget retarget = Retarget.User;
    [SerializeReference, TypeInstance] private ICombatEffect effect;

    protected abstract void SubscribeCondition(Character character);
    protected abstract void UnsubscribeCondition(Character character);

    public void Subscribe(Character character) => SubscribeCondition(character);
    public void Unsubscribe(Character character) => UnsubscribeCondition(character);

    protected void Trigger(CombatArgs args) {
        if (!condition.Match(args)) return;

        var newTgt = retarget == Retarget.User ? args.user : args.target;
        var triggerArgs = args.Chain(this, newTgt);
        effect.PrepareArgs(triggerArgs);
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