using System.Collections;
using System.Linq;
using UnityEngine;

public interface ICombatPhase {
    public IEnumerator Execute(CombatManager cm);
}

public class SetupPhase : ICombatPhase {
    public IEnumerator Execute(CombatManager cm) {
        cm.combatUI.combatPanel.SetActive(true);
        cm.menuPanel.SetActive(false);
        cm.combatUI.consumablesView.SetData(cm.consumables);
        cm.characterList.Clear();
        cm.turnCount = 1;
        cm.characterList = cm.allies.Concat(cm.enemies).OrderByDescending(c => c.derivedStats.speed.currentValue).ToList();
        cm.maxSpeed = cm.characterList.Max(c=> c.derivedStats.speed.currentValue);
        cm.combatUI.ShowCharacters(cm.characterList);
        
        yield break;
    }
}

public class WaitActionPhase : ICombatPhase {
    public IEnumerator Execute(CombatManager cm) {
        cm.currentCharacter = null;
        var fastest = cm.characterList[0];
        while (cm.currentCharacter == null) {
            foreach (var newChar in cm.characterList) {
                newChar.actionPoints.Value += newChar.derivedStats.speed.currentValue * Time.deltaTime;
                if (fastest.actionPoints.Value < newChar.actionPoints.Value) {
                    fastest = newChar;
                }
            }
            
            if (fastest.actionPoints.Value >= cm.maxSpeed) {
                cm.currentCharacter = fastest;
                cm.currentCharacter.actionPoints.Value -= cm.maxSpeed;
            }
            yield return null;
        }
    }
}

public class CharacterPhase : ICombatPhase {
    public IEnumerator Execute(CombatManager cm) {
        cm.selectedSkill = null;
        cm.selectedTarget = null; 
        cm.currentCharacter.OnStartTurn?.Invoke(cm.currentCharacter);
        cm.combatUI.skillPanel.gameObject.SetActive(false);
        if (cm.currentCharacter.derivedStats.health.currentValue == 0) {
            Debug.Log(cm.currentCharacter.characterName + " can't play");
            cm.currentCharacter.OnEndTurn?.Invoke(cm.currentCharacter);
            yield break;
        }
        
        if (cm.enemies.Contains(cm.currentCharacter)) {
            //Vez do Inimigo
            if (cm.selectedTarget != cm.skipTurnFlag) {
                yield return cm.enemyBehaviour.EnemyTurn(cm);
                if (cm.selectedTarget != cm.skipTurnFlag) {
                    yield return ExecuteSelectedSkill(cm, cm.currentCharacter);
                }
            }
        } else {
            //Vez do Player
            Debug.Log("Player Turn: " + cm.currentCharacter.characterName);
            cm.combatUI.actionsPanel.SetActive(true);
            if(cm.currentCharacter.basicAttack.Count > 1) cm.combatUI.secondBasicAttack.SetActive(true);
            else cm.combatUI.secondBasicAttack.SetActive(false);
            cm.combatUI.ShowSkills(cm.currentCharacter, cm);
            yield return new WaitUntil(()=> cm.selectedTarget != null);
            if (cm.selectedTarget != cm.skipTurnFlag) {
                yield return ExecuteSelectedSkill(cm, cm.currentCharacter);
            }
        }
        cm.currentCharacter.OnEndTurn?.Invoke(cm.currentCharacter);
    }

    public IEnumerator ExecuteSelectedSkill(CombatManager cm, Character user) {
        user.derivedStats.mana.AddClampedBaseValue(-cm.selectedSkill.cost);
        cm.combatUI.selectTargetPanel.SetActive(false);
        cm.combatUI.actionsPanel.SetActive(false);
        cm.combatUI.ResetSelections();
        cm.combatUI.ShowSelection(cm.selectedSkill.animation.GetAffectedTargets(user, cm.selectedTarget).ToList());
        if (cm.selectedTarget != null) Debug.Log("Target Selected: " + cm.selectedTarget.characterName);
        var usedSlot = cm.consumables.slots.FirstOrDefault(s => s.item.skillEffect == cm.selectedSkill);
        if (usedSlot != null) cm.consumables.Remove(usedSlot.item, 1);
        yield return cm.selectedSkill.UseSkill(user, cm.selectedTarget, cm);
    }
}

public class CombatEvents : ICombatPhase {
    public IEnumerator Execute(CombatManager cm) {
        cm.combatUI.ResetSelections();
        while (cm.combatEvents.TryDequeue(out var _event)) {
            yield return _event;
        }
    }
}

public class CheckResultPhase : ICombatPhase {
    public IEnumerator Execute(CombatManager cm) {
        var enemiesAlive = cm.enemies.Count;
        foreach (var enemy in cm.enemies) {
            if (enemy.derivedStats.health.currentValue <= 0) enemiesAlive--;
        }
        
        var alliesAlive = cm.allies.Count;
        foreach (var ally in cm.allies) {
            if (ally.derivedStats.health.currentValue <= 0) alliesAlive--;
        }
        
        if (enemiesAlive == 0 || alliesAlive == 0) {
            Debug.Log("Combat Ended");
            cm.gameoverPanel.SetActive(true);
            cm.FinishCombat(alliesAlive > 0);
        }
        yield break;
    }
}