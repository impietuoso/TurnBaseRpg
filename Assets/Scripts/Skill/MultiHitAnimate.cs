using System;
using System.Collections;
using System.Collections.Generic;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[Serializable]
public class MultiHitAnimate : ISkillAnimation {
    public float radius = 10;
    public bool singleTarget;
    public bool targetEnemy;
    public bool targetDead;
    public Vector2Int hitCount = new (1, 1);
    public float damageDelay = 1;
    public float hitDelay = 1;
    public GameObject castingParticle;
    public GameObject skillParticle;

    public bool NeedTarget => true;

    public IEnumerable<ITarget> GetAffectedTargets(Character user, ITarget target) {
        if (singleTarget) {
            yield return target;
            yield break;
        }
        foreach (var newChar in user.CombatController.Characters)
            if (InRange(target.Position, newChar.Position))
                if (ValidateTarget(user, newChar))
                    yield return newChar;
    }

    private bool InRange(Vector3 a, Vector3 b) => (a - b).magnitude < radius;

    public IEnumerator Play(Skill skill, ActionArgs aArgs) {
        if (castingParticle) {
            var vfxParent = aArgs.User.SpriteRenderer.transform;
            var particle = UnityEngine.Object.Instantiate(castingParticle, vfxParent);
            yield return new WaitWhile(() => particle);
        } else
            yield return new WaitForSeconds(0.1f);

        var newHitCount = UnityEngine.Random.Range(hitCount.x, hitCount.y + 1);
        var executores = new List<Coroutine>();

        foreach (var newTarget in GetAffectedTargets(aArgs.User, aArgs.Target)) {
            var dmgRoutine = SingleTargetDamage(skill, aArgs, newTarget, newHitCount);
            executores.Add(aArgs.User.StartCoroutine(dmgRoutine));
        }

        foreach (var exe in executores)
            yield return exe;

        if (hitCount.y > 1) Debug.Log(newHitCount + " Hits");
    }

    public IEnumerator SingleTargetDamage(Skill skill, ActionArgs aArgs, ITarget tgt, int newHitCount) {
        if (tgt is not Character target) yield break;
        for (var i = 0; i < newHitCount; i++) {

            var args = new CombatArgs();
            args.actionArgs = aArgs;
            args.skill = skill;
            args.target = target;
            args.user = aArgs.User;
            args.source = this;

            foreach (var effect in skill.skillEffects)
                effect.Prepare(args);

            if (skillParticle) {
                var parent = target.SpriteRenderer.transform;
                UnityEngine.Object.Instantiate(skillParticle, parent);
            } else
                Debug.LogError("No Particle, add it to: " + skill.skillName, skill);

            yield return new WaitForSeconds(damageDelay);
            args.Resolve();
            yield return new WaitForSeconds(hitDelay);
        }

        yield return new WaitForSeconds(1.2f);
    }

    public bool ValidateTarget(Character user, ITarget tgt) {
        if (tgt is not Character target) return false;
        var sameTeam = user.isAlly == target.isAlly;
        var alive = target.derivedStats.health.currentValue > 0;
        return sameTeam ^ targetEnemy && alive ^ targetDead;
    }
}