using System;
using System.Collections;
using Unity.VisualScripting;
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
    private Slider speedView;
    [SerializeField]
    private Image elementIcon;
    [SerializeField]
    private StatusEffectListView statusEffectView;
    public Color normalSpeedColor;
    public Color fastSpeedColor;
    public Color slowSpeedColor;
    
    private void ChangeSpeedColor(int value) {
        if (!speedView) return;
        var baseValue = owner.derivedStats.speed.baseValue;
        var sliderColor = value == baseValue ? normalSpeedColor : value > baseValue ? fastSpeedColor : slowSpeedColor;
        speedView.fillRect.GetComponent<Image>().color = sliderColor;
    }

    public void SetCharacterUIValues(Character newChar) {
        owner = newChar;

        if (uiSprite) uiSprite.sprite = newChar.uiSprite;
        if (characterSprite) characterSprite.sprite = newChar.characterSprite;
        owner.StatusEffectList.OnStatusAdded += HandleNewStat;
        if (healthView) healthView.SetStat(newChar.derivedStats.health);
        if (manaView) manaView.SetStat(newChar.derivedStats.mana);
        if (shieldView) shieldView.SetStat(newChar.derivedStats.shield);
        
        if (elementIcon) elementIcon.sprite = newChar.element.elementSprite;
        if (elementIcon) elementIcon.color = newChar.element.elementColor;
        
        if (speedView) speedView.value = newChar.actionPoints.Value;
        ChangeSpeedColor(owner.derivedStats.speed.currentValue);
        if (speedView) speedView.maxValue = CombatManager.instance.maxSpeed;
        if (speedView) owner.actionPoints.OnChange += UpdateSpeedSlider;
        
        if(statusEffectView) statusEffectView.SetData(newChar.StatusEffectList);

        owner.OnResolveDefend += HandleHealthChanged;
        owner.derivedStats.speed.OnChange += ChangeSpeedColor;
        owner.OnStartTurn += ActiveArrow;
        owner.OnEndTurn += DisableArrow;
        gameObject.SetActive(true);
    }

    private void UpdateSpeedSlider(float newValue) {
        speedView.value = newValue;
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
        owner.derivedStats.speed.OnChange -= ChangeSpeedColor;
    }

    private void HandleHealthChanged(CombatArgs args) {
        Color damageColor = args.result switch {
            { miss: true } => Color.white,
            { resistStatus: true } => Color.orange,
            { isCrit: true } => Color.yellow,
            { deltaHp: > 0 } => Color.green,
            { deltaHp: < 0 } => Color.red,
            { deltaMp: > 0 } => Color.blue,
            { deltaMp: < 0 } => Color.blueViolet,
            { deltaShield: > 0 } => Color.cyan,
            { deltaShield: < 0 } => Color.gray3,
            _ => Color.deepPink
        };

        var statusApplied = args.statusEffects.Count > 0 && !args.result.resistStatus;
        if (statusApplied && args.damage == 0) return;
            
        var popupValue = args.result.deltaShield + args.result.deltaHp;
        
        if (args.user == args.target && popupValue == 0) {
            return;
        }

        var popupText = args.result.miss ? "Miss" : popupValue.ToString();
        if (!args.result.miss && args.result.resistStatus) {
            if (popupValue == 0)
                popupText = "Resist";
            else {
                popupText += "\nResist";
            }
        }
        

        if (targetButton) CombatManager.instance.combatUI.callPopup.CreatePopup(popupText, damageColor, transform);

        if (args.result.isFatal && characterSprite) characterSprite.color = new Color(1, 1, 1, 0.5f);
        if (args.result.isRevive && characterSprite) characterSprite.color = new Color(1, 1, 1, 1f);
    }
    
    private void HandleNewStat(Status newStatus) {
        var NewStatusPopup = CombatManager.instance.combatUI.callPopup.Pop(newStatus.statusName, newStatus.source.statusPopupColor, transform, 0);
        var genericEvent = new GenericCombatEvent(NewStatusPopup);
        CombatManager.instance.combatEvents.Enqueue(genericEvent);
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