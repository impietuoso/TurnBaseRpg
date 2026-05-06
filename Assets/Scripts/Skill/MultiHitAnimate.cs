using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiHitAnimate : ISkillAnimation {
    public bool singleTarget;
    public bool targetEnemy;
    public bool targetDead;
    public Vector2Int hitCount = new Vector2Int(1, 1);
    public float hitDelay = 1;
    public bool TrySkipSelection(Character user, Skill skill) => false;

    public IEnumerable<Character> GetAffectedTargets(Character user, Character target) {
        if (singleTarget) {
            yield return target;
        } else {
            foreach (var newTarget in CombatManager.instance.characterList) {
                if (ValidateTarget(user, newTarget)) yield return newTarget;
            }
        }
    }

    public IEnumerator Play(Skill skill, Character user, Character target, CombatManager cm) {
        yield return new WaitForSeconds(0.1f);

        int newHitCount = UnityEngine.Random.Range(hitCount.x, hitCount.y + 1);

        List<Coroutine> executores = new();

        foreach (var newTarget in GetAffectedTargets(user, target)) {
            executores.Add(cm.StartCoroutine(SingleTargetDamage(skill, user, newTarget, newHitCount)));
        }

        foreach (var exe in executores) {
            yield return exe;
        }
        
        if (hitCount.y > 1) Debug.Log(newHitCount + " Hits");
    }

    public IEnumerator SingleTargetDamage(Skill skill, Character user, Character target, int newHitCount) {
        for (int i = 0; i < newHitCount; i++) {

            CombatArgs args = new CombatArgs();
            args.skill = skill;
            args.target = target;
            args.user = user;
            args.source = this;

            foreach (var effect in skill.skillEffects) {
                effect.Prepare(args);
            }

            args.Resolve();
            yield return new WaitForSeconds(hitDelay);
        }
        yield return new WaitForSeconds(1.2f);
    }

    public bool ValidateTarget(Character user, Character target) {
        bool sameTeam = user.team == target.team;
        bool alive = target.derivedStats.health.currentValue > 0;
        return sameTeam ^ targetEnemy && alive ^ targetDead;
    }
}