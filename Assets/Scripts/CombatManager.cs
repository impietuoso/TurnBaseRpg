using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour {
    public static CombatManager instance;
    public Skill basicDefense;
    public CombatUI combatUI;
    public GameObject menuPanel;
    public GameObject gameoverPanel;
    [Space(5)]
    [Header("Runtime")]
    public List<Character> allies = new();
    public List<Character> enemies = new();
    public List<Character> turnOrder = new();
    public Character currentCharacter => turnOrder[currentCharacterIndex];
    public int currentCharacterIndex;
    [SerializeField]
    private int turnCount = 0;
    public ListInventory<Consumable> consumables;
    private EnemyBehaviour enemyBehaviour = new();
    public Queue<IEnumerator> combatEvents = new();

    private void Awake() {
        instance = this;
    }

    public void StartCombat(EnemyEncounter newEnemies, IEnumerable<PartyMember> party) {
        combatUI.combatPanel.SetActive(true);
        menuPanel.SetActive(false);
        currentCharacterIndex = -1;

        combatUI.consumablesView.SetData(consumables);
        
        // Reset and clone allies
        allies.Clear();
        foreach (var ally in party) {
            if (ally == null) {
                continue;
            }
            allies.Add(new(ally, "Player"));
        }

        // Clone enemies
        enemies.Clear();
        foreach (var newEnemy in newEnemies.enemyList) {
            if (newEnemy == null) {
                continue;
            }
            enemies.Add(new(newEnemy, "Enemy"));
        }

        turnOrder.Clear();
        turnCount = 0;
        SetTurn();
    }

    private void SetTurn() {
        turnCount++;
        turnOrder = allies.Concat(enemies).OrderByDescending(c => c.derivedStats.speed.currentValue).ToList();
        //foreach (var person in turnOrder) {
        //    person.UpdateCombatValues();
        //}
        combatUI.ShowCharacters(turnOrder);
        TurnManager();
    }

    [ContextMenu("Execute Turn Manager")]
    public void TurnManager() {
        //Verify 
        if (combatEvents.Count > 0) {
            StartCoroutine(ExecuteCombatEvents());
            return;
        }
        
        SetNextCharacter();
        combatUI.skillPanel.gameObject.SetActive(false);

        var enemiesAlive = enemies.Count;
        foreach (var enemy in enemies) {
            if (enemy.derivedStats.health.currentValue <= 0) enemiesAlive--;
        }
        
        var alliesAlive = allies.Count;
        foreach (var ally in allies) {
            if (ally.derivedStats.health.currentValue <= 0) alliesAlive--;
        }
        
        if (enemiesAlive == 0 || alliesAlive == 0) {
            Debug.Log("Combat Ended");
            gameoverPanel.SetActive(true);
            return;
        }

        if (turnOrder[currentCharacterIndex].derivedStats.health.currentValue == 0) {
            Debug.Log(turnOrder[currentCharacterIndex].characterName + " can't play");
            TurnManager();
            return;
        }
        
        if (enemies.Contains(turnOrder[currentCharacterIndex])) {
            //Vez do Inimigo
            StartCoroutine(enemyBehaviour.EnemyTurn(this));
        } else {
            //Vez do Player
            Debug.Log("Player Turn: " + turnOrder[currentCharacterIndex].characterName);
            combatUI.actionsPanel.SetActive(true);
            if(turnOrder[currentCharacterIndex].basicAttack.Count > 1) combatUI.secondBasicAttack.SetActive(true);
            else combatUI.secondBasicAttack.SetActive(false);
            combatUI.ShowSkills(turnOrder[currentCharacterIndex], this);
        }
    }

    public void UsingSkillOnTarget(Character user, Skill skill, Character target) {
        user.derivedStats.mana.AddClampedBaseValue(-skill.cost);
        combatUI.selectTargetPanel.SetActive(false);
        combatUI.actionsPanel.SetActive(false);
        combatUI.ResetSelections();
        combatUI.ShowSelection(skill.animation.GetAffecterTargets(user, target).ToList());
        skill.UseSkill(user, target, this);
        if(target != null) Debug.Log("Target Selected: " + target.characterName);
        var usedSlot = consumables.slots.FirstOrDefault(s => s.item.skillEffect == skill);
        if (usedSlot != null) consumables.Remove(usedSlot.item, 1);
    }

    public void SetNextCharacter() {
        if (currentCharacterIndex >= 0) {
            if(currentCharacter.derivedStats.health.currentValue != 0) currentCharacter.OnEndTurn?.Invoke(currentCharacter);
        }
            
        currentCharacterIndex++;
        if (currentCharacterIndex >= turnOrder.Count) {
            turnOrder.Sort((b, a) => a.derivedStats.speed.currentValue.CompareTo(b.derivedStats.speed.currentValue));
            currentCharacterIndex = 0;
            turnCount++;
        }
        
        if(currentCharacter.derivedStats.health.currentValue != 0) currentCharacter.OnStartTurn?.Invoke(currentCharacter);
    }

    public void ReloadScene() {
        SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator ExecuteCombatEvents() {
        while (combatEvents.TryDequeue(out var _event)) {
            yield return _event;
        }
        TurnManager();
    }
}