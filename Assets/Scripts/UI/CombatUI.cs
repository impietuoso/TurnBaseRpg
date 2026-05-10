using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {
    [Header("UI Components")]
    public GameObject combatPanel;
    public GameObject actionsPanel;
    public GameObject skillPanel;
    public GameObject selectTargetPanel;
    public GameObject secondBasicAttack;
    public GameObject currentActionPanel;
    public TextMeshProUGUI currentActionText;
    public Button skillTemplate;
    public IInventoryView consumablesView;
    public TextMeshProUGUI currentSkillNameText;
    public TextMeshProUGUI currentSkillDescriptionText;
    public CharacterUITemplate smallAllyTemplate;
    public CharacterUITemplate allyCharacterTemplate;
    public CharacterUITemplate enemyCharacterTemplate;
    public StatusEffectListView statusEffectsDescription;
    public List<CharacterUITemplate> characters = new();
    public CallPopupText callPopup;
    public Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    public void ShowSkills(Character newChar, CombatManager combatManager) {
        foreach (Transform child in skillTemplate.transform.parent) {
            if (child.gameObject != skillTemplate.gameObject) {
                Destroy(child.gameObject);
            }
        }

        foreach (var skill in newChar.skills) {
            if (skill == null) {
                continue;
            }
            
            Button skillButton = Instantiate(skillTemplate, skillTemplate.transform.parent);
            skillButton.GetComponent<ListTemplateView>().UpdateTemplateUI(skill);
            var activate = skill.Available(newChar);
            skillButton.interactable = activate;
            skillButton.gameObject.SetActive(true);
            if (!activate) continue;
            skillButton.onClick.RemoveAllListeners();
            skillButton.onClick.AddListener(() => PrepareSkill(newChar, skill));
        }
    }

    public void PrepareSkill(Character user, Skill skill) {
        if (user.derivedStats.mana.currentValue >= skill.cost) {
            skillPanel.SetActive(false);
            selectTargetPanel.SetActive(true);
            currentSkillNameText.text = "[" + skill.skillName + "]";
            currentSkillDescriptionText.text = skill.skillDescription;
            if (skill.animation.TrySkipSelection(user, skill)) {
                CombatManager.instance2.UsingSkillOnTarget(user, skill, user);
            } else VerifyTargets(user, skill);
        } else {
            Debug.Log("Don't have enough Mana");
        }
    }

    public void PrepareAttack1ForCurrentPlayer() {
        var currentPlayer = CombatManager.instance2.currentCharacter;
        PrepareSkill(currentPlayer, currentPlayer.basicAttack[0]);
    }
    
    public void PrepareAttack2ForCurrentPlayer() {
        var currentPlayer = CombatManager.instance2.currentCharacter;
        PrepareSkill(currentPlayer, currentPlayer.basicAttack[1]);
    }
    
    public void PrepareDefenseForCurrentPlayer() {
        var currentPlayer = CombatManager.instance2.currentCharacter;
        PrepareSkill(currentPlayer, CombatManager.instance2.basicDefense);
    }

    public void PrepareSkillForCurrentPlayer(IItemView itemView) {
        var currentPlayer = CombatManager.instance2.currentCharacter;
        PrepareSkill(currentPlayer, ((Consumable)itemView.Data).skillEffect);
    }
    
    public void VerifyTargets(Character user, Skill skill) {
        var searchList = characters;
        foreach (var target in searchList) {
            bool active = skill.animation.ValidateTarget(user ,target.owner);
            if (active) {
                target.SetButtonAction(() => CombatManager.instance2.UsingSkillOnTarget(user, skill, target.owner));
            } else {
                target.SetButtonAction(null);
            }
        }
    }

    public void ResetSelections() {
        foreach (var target in characters) {
            target.SetButtonAction(null);
        }
    }

    public void ShowSelection(List<Character> chars) {
        foreach (var newChar in characters) {
            if(chars.Contains(newChar.owner)) newChar.EnableSelection();
        }
    }
    
    public void ShowCharacters(List<Character> newCharacters) {
        RemoveOldTemplates();

        foreach (Character newChar in newCharacters) {
            GameObject character = Instantiate(
                newChar.team == "Player"
                    ? allyCharacterTemplate.gameObject
                    : enemyCharacterTemplate.gameObject,
                newChar.team == "Player"
                    ? allyCharacterTemplate.transform.parent
                    : enemyCharacterTemplate.transform.parent);
            var template = character.GetComponent<CharacterUITemplate>();
            template.SetCharacterUIValues(newChar);

            if (template.owner.team == "Player") {
                GameObject smallTemplate = Instantiate(
                    smallAllyTemplate.gameObject,
                    smallAllyTemplate.transform.parent);
                smallTemplate.GetComponent<CharacterUITemplate>().SetCharacterUIValues(newChar);
            }
            characters.Add(template);
        }
    }

    void RemoveOldTemplates() {
        characters.Clear();

        foreach (Transform child in allyCharacterTemplate.transform.parent) {
            if (child.gameObject != allyCharacterTemplate.gameObject) {
                Destroy(child.gameObject);
            }
        }

        foreach (Transform child in enemyCharacterTemplate.transform.parent) {
            if (child.gameObject != enemyCharacterTemplate.gameObject) {
                Destroy(child.gameObject);
            }
        }
    }

    public void ShowCurrentStatusEffects(StatusEffectListView statusEffectListView) {
        statusEffectsDescription.SetData(statusEffectListView.owner);
    }

    public void ShowCurrentAction(string userName, string actionName) {
        if (currentActionPanel.activeInHierarchy)
            currentActionPanel.SetActive(false);
        currentActionPanel.SetActive(true);
        currentActionText.text = userName + " uses " + actionName;
    }

    public Vector2 GetCharacterWorldPosition(Character character) {
        Vector2 position = Vector2.zero;
        foreach (var newChar in characters) {
            //if (newChar.owner == character) position = cam.ScreenToWorldPoint(newChar.transform.position);
            if (newChar.owner == character) position = newChar.transform.position;
        }
        return position;
    }
}
