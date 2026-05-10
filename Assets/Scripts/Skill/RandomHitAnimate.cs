using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool TrySkipSelection(Character user, Skill skill) => false;

    public IEnumerator Play(Skill skill, Character user, Character target, CombatManager cm) {
        var ui = CombatManager.instance.combatUI;
        if (castingParticle) {
            var particle = UnityEngine.Object.Instantiate(
                castingParticle,
                ui.GetCharacterWorldPosition(user),
                Quaternion.identity);
            yield return new WaitWhile(() => particle);
        } else {
            yield return new WaitForSeconds(0.1f);
        }

        int newHitCount = Random.Range(hitCount.x, hitCount.y + 1);

        List<Character> targets = new(GetAffectedTargets(user, target));
        yield return cm.StartCoroutine(SingleTargetDamage(skill, user, targets, newHitCount));
        
        if (hitCount.y > 1) Debug.Log(newHitCount + " Hits");
    }

    public IEnumerable<Character> GetAffectedTargets(Character user, Character target) {
        foreach (var newTarget in CombatManager.instance.characterList) {
            if (ValidateTarget(user, newTarget)) yield return newTarget;
        }
    }
    
    public IEnumerator SingleTargetDamage(Skill skill, Character user, List<Character> targets, int newHitCount) {
        for (int i = 0; i < newHitCount; i++) {

            CombatArgs args = new CombatArgs();
            args.skill = skill;
            args.target = targets[Random.Range(0, targets.Count)];
            args.user = user;
            args.source = this;

            foreach (var effect in skill.skillEffects) {
                effect.Prepare(args);
            }

            var ui = CombatManager.instance.combatUI;
            UnityEngine.Object.Instantiate(skillParticle, ui.GetCharacterWorldPosition(args.target), Quaternion.identity);
            yield return new WaitForSeconds(damageDelay);
            
            args.Resolve();
            yield return new WaitForSeconds(hitDelay);
        }
    }
    
    public bool ValidateTarget(Character user, ITarget tgt) {
        if (tgt is not Character target) return false;
        bool sameTeam = user.team == target.team;
        bool alive = target.derivedStats.health.currentValue > 0;
        return sameTeam ^ targetEnemy && alive ^ targetDead;
    }
}
