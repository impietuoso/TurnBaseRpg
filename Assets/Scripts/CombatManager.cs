using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obsolete]
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
    [NonSerialized]
    public List<Character> characterList = new();
    [NonSerialized]
    public Character currentCharacter;
    [SerializeField]
    public int turnCount = 0;
    public ListInventory<Consumable> consumables;
    [Header("Combat Phases")]
    public EnemyBehaviour enemyBehaviour = new();
    public Queue<ICombatPhase> combatEvents = new();
    public List<ICombatPhase> setupPhases = new();
    public List<ICombatPhase> loopPhases = new();
    public List<ICombatPhase> endPhases = new();
    public Coroutine currentPhase;
    [HideInInspector]
    public bool combatWon;
    [Header("Debug Variables")]
    public Skill selectedSkill;
    [NonSerialized]
    public Character selectedTarget;
    public readonly Character skipTurnFlag = new();
    public int maxSpeed;

    private void Awake() {
        instance = this;
        setupPhases.Add(new SetupPhase());
        loopPhases.Add(new WaitActionPhase());
        loopPhases.Add(new CharacterPhase());
        loopPhases.Add(new CombatEvents());
        loopPhases.Add(new CheckResultPhase());
    }

    public void StartCombat(EnemyEncounter newEnemies, IEnumerable<PartyMember> party) {
        // Reset and clone allies
        allies.Clear();
        foreach (var ally in party) {
            if (ally == null) {
                continue;
            }

            //allies.Add(new(ally, "Player"));
        }

        // Clone enemies
        enemies.Clear();
        foreach (var newEnemy in newEnemies.enemyList) {
            if (newEnemy == null) {
                continue;
            }

            //enemies.Add(new(newEnemy, "Enemy"));
        }

        currentPhase = StartCoroutine(CombatLoop());
    }

    public void FinishCombat(bool won) {
        combatWon = won;
        StopCoroutine(currentPhase);
        StartCoroutine(CombatEnd());
    }

    public IEnumerator CombatLoop() {
        foreach (var phase in setupPhases) {
            yield return phase.Execute(this);
        }

        while (true) {
            foreach (var phase in loopPhases) {
                yield return phase.Execute(this);
            }
        }
    }

    public IEnumerator CombatEnd() {
        foreach (var phase in endPhases) {
            yield return phase.Execute(this);
        }
    }

    [ContextMenu("Skip Turn ( ͡° ͜ʖ ͡°)")]
    public void SkipTurn() {
        selectedTarget = skipTurnFlag;
        combatUI.actionsPanel.SetActive(false);
    }

    public void UsingSkillOnTarget(Character user, Skill skill, Character target) {
        selectedSkill = skill;
        selectedTarget = target;
    }

    public void ReloadScene() {
        SceneManager.LoadSceneAsync(0);
    }
}

public class GenericCombatEvent : ICombatPhase {
    private IEnumerator function;

    public GenericCombatEvent(IEnumerator function) {
        this.function = function;
    }

    public IEnumerator Execute(CombatManager cm) {
        yield return function;
    }
}
