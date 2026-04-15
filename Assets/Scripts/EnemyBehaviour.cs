using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviour {
    private int GetWeightedRandomIndex(float[] weights) {
        float totalWeight = weights.Sum();
        float randomValue = Random.Range(0, totalWeight);
        float currentSum = 0;

        for (int i = 0; i < weights.Length; i++) {
            currentSum += weights[i];
            if (randomValue <= currentSum) {
                return i;
            }
        }

        return 0;
    }

    public IEnumerator EnemyTurn(CombatManager cm) {
        Character currentEnemy = cm.currentCharacter;
        Debug.Log("Enemy Turn: " + currentEnemy.characterName);

        yield return new WaitForSeconds(0.3f);

        List<Skill> availableSkills = currentEnemy.skills.Where(s => s.Available(currentEnemy)).ToList();
        Skill selectedSkill = null;

        if (availableSkills.Count > 0) {
            // Usa Skill (60%), Ataca (30%) ou Defende (10%)
            int choice = GetWeightedRandomIndex(new float[] { 60, 30, 10 });
            if (choice == 0) {
                selectedSkill = availableSkills[Random.Range(0, availableSkills.Count)];
            } else if (choice == 1) {
                selectedSkill = currentEnemy.basicAttack[0];
            } else {
                selectedSkill = cm.basicDefense;
            }
        } else {
            // Ataca (80%) ou Defende (20%)
            int choice = GetWeightedRandomIndex(new float[] { 80, 20 });
            var attack = currentEnemy.basicAttack[0];
            selectedSkill = choice == 0 ? attack : cm.basicDefense;
        }
        
        yield return ExecuteSkill(currentEnemy, selectedSkill, cm);
    }

    private IEnumerator ExecuteSkill(Character user, Skill skill, CombatManager cm) {
        List<Character> potentialTargets = new List<Character>();
        foreach (var template in cm.combatUI.characters) {
            if (skill.animation.ValidateTarget(user, template.owner)) {
                potentialTargets.Add(template.owner);
            }
        }

        Character targetCharacter = null;
        if (potentialTargets.Count > 0) {
            targetCharacter = potentialTargets[UnityEngine.Random.Range(0, potentialTargets.Count)];

            // Highlight target in UI
            foreach (var template in cm.combatUI.characters) {
                if (template.owner == targetCharacter) {
                    template.ShowSelectedTarget(true);
                    break;
                }
            }
        }

        yield return new WaitForSeconds(1f);
        
        user.derivedStats.mana.AddClampedBaseValue(skill.cost);
        skill.UseSkill(user, targetCharacter, cm);
    }
}
