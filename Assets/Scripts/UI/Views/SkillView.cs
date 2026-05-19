using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : DataView<Skill> {
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Image icon;
    public CanvasGroup interactable;

    private void Start() {
        if (interactable && !Data) interactable.interactable = false;
    }

    protected override void Subscribe() {
        if (skillNameText) skillNameText.text = Data.DisplayName;
        if (skillNameText) skillNameText.color = Data.element.elementColor;
        if (descriptionText) descriptionText.text = Data.skillDescription;
        if (costText) costText.text = Data.Cost + " MP";
        if (icon) icon.overrideSprite = Data.Icon;
        if (interactable) interactable.interactable = true;
    }

    protected override void Unsubscribe() {
        if (interactable) interactable.interactable = false;
        if (skillNameText) skillNameText.text = "Empty";
        if (skillNameText) skillNameText.color = Color.white;
        if (descriptionText) descriptionText.text = "-";
        if (costText) costText.text = "-";
        if (icon) icon.overrideSprite = null;
    }
}
