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
        if (interactable && !Data) interactable.interactable = false;
    }

    protected override void Subscribe() {
        if (icon) icon.overrideSprite = Data.sprite;
        if (nameText) nameText.text = Data.displayName;
        if (descriptionText) descriptionText.text = Data.description;
        if (bonusText) bonusText.text = Data.BonusText();
        if (equipments) equipments.SetData(Data);
        if (interactable) interactable.interactable = true;
    }

    protected override void Unsubscribe() {
        if (interactable) interactable.interactable = false;
        if (icon) icon.overrideSprite = null;
        if (nameText) nameText.text = "Empty";
        if (bonusText) bonusText.text = "-";
        if (descriptionText) descriptionText.text = "-";
    }
}
