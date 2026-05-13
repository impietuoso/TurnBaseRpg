using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class SpreadEffect : ICombatEffect {
    [SerializeReference, TypeDropdown]
    public ICombatEffect effect;
    public float spreadDelay;
    public float radius = 10f;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        foreach (var newTarget in args.user.CombatController.Characters) {
            if (!ValidateTarget(args.target, newTarget)) continue;
            var chain = args.Chain(this, newTarget);
            effect.PrepareArgs(chain);
            args.OnResolve += _ => args.CombatCoroutine(ResolveSpread(chain));
        }
    }

    public IEnumerator ResolveSpread(CombatArgs newArgs) {
        yield return new WaitForSeconds(spreadDelay);
        newArgs.Resolve();
    }
    
    public bool ValidateTarget(Character user, Character target) {
        var sameTeam = user.isAlly == target.isAlly;
        var alive = target.Health.Current > 0;
        var notSelf = target != user;
        return sameTeam && alive && notSelf;
    }
}