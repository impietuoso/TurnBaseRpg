using System.Collections.Generic;
using System.Linq;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

public class EnemyBehaviour {
    private int GetWeightedRandomIndex(float[] weights) {
        var totalWeight = weights.Sum();
        var randomValue = Random.Range(0, totalWeight);
        float currentSum = 0;

        for (var i = 0; i < weights.Length; i++) {
            currentSum += weights[i];
            if (randomValue <= currentSum)
                return i;
        }

        return 0;
    }

    public void ChooseNextAction(CombatController cc, Character user) {
        Debug.Log("Enemy Turn: " + user.Creature.DisplayName);
        var availableSkills = user.Creature.Skills.Where(s => s.Available(user)).ToList();
        Skill skill;

        if (availableSkills.Count > 0) {
            // Usa Skill (60%), Ataca (30%) ou Defende (10%)
            var choice = GetWeightedRandomIndex(new float[] { 60, 30, 10 });
            if (choice == 0)
                skill = availableSkills[Random.Range(0, availableSkills.Count)];
            else if (choice == 1)
                skill = user.basicAttack[0];
            else
                skill = cc.BasicDefendSkill;
        } else {
            // Ataca (80%) ou Defende (20%)
            var choice = GetWeightedRandomIndex(new float[] { 80, 20 });
            var attack = user.basicAttack[0];
            skill = choice == 0 ? attack : cc.BasicDefendSkill;
        }

        var potentialTargets = new List<Character>();
        foreach (var character in cc.Characters)
            if (skill.animation.ValidateTarget(user, character))
                potentialTargets.Add(character);

        var target = potentialTargets[Random.Range(0, potentialTargets.Count)];
        user.NextAction = potentialTargets.Count == 0 ? null : new ActionArgs(skill, user, target);
    }
}