using System;
using System.Collections;
using UnityEngine;
public class SpreadEffect : ISkillEffect {
    [SerializeReference, TypeDropdown(typeof(ISkillEffect))]
    public ISkillEffect effect;
    public float spreadDelay;

    public void Prepare(CombatArgs args) {
        foreach (var newTarget in CombatManager.instance.characterList) {
            if (ValidateTarget(args.target, newTarget)) {
                CombatArgs newArgs = new();
                newArgs.skill = args.skill;
                newArgs.target = newTarget;
                newArgs.user = args.user;
                newArgs.source = args.source;
                newArgs.unavoidable = true;
                effect.Prepare(newArgs);
                args.OnResolve += _=> CombatManager.instance.StartCoroutine(ResolveSpread(newArgs));
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
