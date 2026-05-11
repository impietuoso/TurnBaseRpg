using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RandomHitAnimate : ISkillAnimation {
    public bool targetEnemy;
    public bool targetDead;
    public Vector2Int hitCount = new (1, 1);
    public float damageDelay = 1;
    public float hitDelay = 1;
    public GameObject castingParticle;
    public GameObject skillParticle;

    public bool NeedTarget => true;

    public IEnumerator Play(Skill skill, ActionArgs aArgs) {
        if (castingParticle) {
            var particle = UnityEngine.Object.Instantiate(
                castingParticle,
                aArgs.Target.Position,
                Quaternion.identity);
            yield return new WaitWhile(() => particle);
        } else 
            yield return new WaitForSeconds(0.1f);

        var newHitCount = Random.Range(hitCount.x, hitCount.y + 1);

        List<Character> targets = new(GetAffectedTargets(aArgs.User, aArgs.Target).OfType<Character>());
        yield return SingleTargetDamage(skill, aArgs, targets, newHitCount);
        
        if (hitCount.y > 1) Debug.Log(newHitCount + " Hits");
    }

    public IEnumerable<ITarget> GetAffectedTargets(Character user, ITarget target) {
        foreach (var newTarget in user.CombatController.Characters) {
            if (ValidateTarget(user, newTarget)) yield return newTarget;
        }
    }
    
    public IEnumerator SingleTargetDamage(Skill skill, ActionArgs aArgs, List<Character> targets, int newHitCount) {
        
        for (var i = 0; i < newHitCount; i++) {
            var args = new CombatArgs();
            args.actionArgs = aArgs;
            args.skill = skill;
            args.target = targets[Random.Range(0, targets.Count)];
            args.user = aArgs.User;
            args.source = this;

            foreach (var effect in skill.skillEffects) {
                effect.Prepare(args);
            }

            UnityEngine.Object.Instantiate(skillParticle,  args.target.Position, Quaternion.identity);
            yield return new WaitForSeconds(damageDelay);
            
            args.Resolve();
            yield return new WaitForSeconds(hitDelay);
        }
    }
    
    public bool ValidateTarget(Character user, ITarget tgt) {
        if (tgt is not Character target) return false;
        var sameTeam = user.team == target.team;
        var alive = target.derivedStats.health.currentValue > 0;
        return sameTeam ^ targetEnemy && alive ^ targetDead;
    }
}
