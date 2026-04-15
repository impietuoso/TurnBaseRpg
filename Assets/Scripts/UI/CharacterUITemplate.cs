using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUITemplate : MonoBehaviour {
    public Character owner;
    [SerializeField]
    private Transform arrow;
    [SerializeField]
    private Button targetButton;
    [SerializeField]
    private Image uiSprite;
    [SerializeField]
    private Image characterSprite;
    [SerializeField]
    private StatView healthView;
    [SerializeField]
    private StatView manaView;
    [SerializeField]
    private StatView shieldView;
    [SerializeField]
    private StatusEffectListView statusEffectView;

    public void SetCharacterUIValues(Character newChar) {
        owner = newChar;

        if (uiSprite) uiSprite.sprite = newChar.uiSprite;
        if (characterSprite) characterSprite.sprite = newChar.characterSprite;

        if (healthView) healthView.SetStat(newChar.derivedStats.health);
        if (manaView) manaView.SetStat(newChar.derivedStats.mana);
        if (shieldView) shieldView.SetStat(newChar.derivedStats.shield);
        
        if(statusEffectView) statusEffectView.SetData(newChar.StatusEffectList);

        owner.OnResolveDefend += HandleHealthChanged;
        owner.StatusEffectList.OnStatusAdded += HandleNewStat;
        owner.OnStartTurn += ActiveArrow;
        owner.OnEndTurn += DisableArrow;
        gameObject.SetActive(true);
    }

    private void ActiveArrow(Character obj) {
        if (!arrow) return;
        arrow.gameObject.SetActive(true);
    }

    private void DisableArrow(Character obj) {
        if (!arrow) return;
        arrow.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        if (owner != null && owner.derivedStats.health != null) {
            owner.OnResolveDefend -= HandleHealthChanged;
            owner.OnStartTurn -= ActiveArrow;
            owner.OnEndTurn -= DisableArrow;
        }
    }

    private void HandleHealthChanged(CombatArgs args) {
        Color damageColor = args.result switch {
            { miss: true } => Color.white,
            { isCrit: true } => Color.yellow,
            { deltaHp: > 0 } => Color.green,
            { deltaHp: < 0 } => Color.red,
            { deltaMp: > 0 } => Color.blue,
            { deltaMp: < 0 } => Color.blueViolet,
            { deltaShield: > 0 } => Color.cyan,
            { deltaShield: < 0 } => Color.gray3,
            _ => Color.deepPink
        };

        var popupValue = args.result.deltaShield + args.result.deltaHp;
        
        if (args.user == args.target && popupValue == 0) {
            return;
        }

        var popupText = args.result.miss ? "Miss" : popupValue.ToString();

        if (targetButton) CombatManager.instance.combatUI.callPopup.CreatePopup(popupText, damageColor, transform);

        if (args.result.isFatal && characterSprite) characterSprite.color = new Color(1, 1, 1, 0.5f);
        if (args.result.isRevive && characterSprite) characterSprite.color = new Color(1, 1, 1, 1f);
    }
    
    private void HandleNewStat(Status newStatus) {
        var NewStatusPopup = CombatManager.instance.combatUI.callPopup.Pop(newStatus.statusName, newStatus.source.statusPopupColor, transform, 0);
        CombatManager.instance.combatEvents.Enqueue(NewStatusPopup);
    }
    
    public void SetButtonAction(Action selectAction) {
        if (!targetButton) return;

        targetButton.onClick.RemoveAllListeners();
        if (selectAction == null) {
            targetButton.interactable = false;
        } else {
            targetButton.interactable = true;
            targetButton.onClick.AddListener(selectAction.Invoke);
        }
    }
    
    public void EnableSelection() {
        targetButton.interactable = true;
    }

    public void ShowSelectedTarget(bool state) {
        if (!targetButton) return;
        targetButton.interactable = state;
    }
}
