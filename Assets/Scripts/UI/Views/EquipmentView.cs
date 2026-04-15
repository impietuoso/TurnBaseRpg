using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : DataView<Equipment> {
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI bonusText;
    public ListView equipments;
    public CanvasGroup interactable;
    
    private void Start() {
        if (interactable && !data) interactable.interactable = false;
    }

    public override void Subscribe() {
        if (icon) icon.overrideSprite = data.sprite;
        if (nameText) nameText.text = data.displayName;
        if (descriptionText) descriptionText.text = data.description;
        if (bonusText) bonusText.text = data.BonusText();
        if (equipments) equipments.SetData(data);
        if (interactable) interactable.interactable = true;
    }

    public override void Unsubscribe() {
        if (interactable) interactable.interactable = false;
        if (icon) icon.overrideSprite = null;
        if (nameText) nameText.text = "Empty";
        if (bonusText) bonusText.text = "-";
        if (descriptionText) descriptionText.text = "-";
    }
}
