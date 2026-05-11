using System;
using System.Collections;
using UnityEngine;
[Serializable]
public class SpreadEffect : ISkillEffect {
    [SerializeReference, TypeDropdown]
    public ISkillEffect effect;
    public float spreadDelay;
    public float radius = 10f;

    public void Prepare(CombatArgs args) {
        foreach (var newTarget in args.user.CombatController.Characters) {
            if (ValidateTarget(args.target, newTarget)) {
                var newArgs = new CombatArgs();
                newArgs.actionArgs = args.actionArgs;
                newArgs.skill = args.skill;
                newArgs.target = newTarget;
                newArgs.user = args.user;
                newArgs.source = args.source;
                newArgs.unavoidable = true;
                effect.Prepare(newArgs);
                args.OnResolve += _=> args.user.CombatController.StartCoroutine(ResolveSpread(newArgs));
            }
        }
    }

    public IEnumerator ResolveSpread(CombatArgs newArgs) {
        yield return new WaitForSeconds(spreadDelay);
        newArgs.Resolve();
    }
    
    public bool ValidateTarget(Character user, Character target) {
        bool sameTeam = user.team == target.team;
        bool alive = target.derivedStats.health.currentValue > 0;
        bool notSelf = target != user;
        return sameTeam && alive && notSelf;
    }
}