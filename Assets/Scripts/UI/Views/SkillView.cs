using System;
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
        if (interactable && !data) interactable.interactable = false;
    }

    public override void Subscribe() {
        if (skillNameText) skillNameText.text = data.skillName;
        if (descriptionText) descriptionText.text = data.skillDescription;
        if (costText) costText.text = data.cost + " MP";
        if (icon) icon.overrideSprite = data.icon;
        if (interactable) interactable.interactable = true;
    }

    public override void Unsubscribe() {
        if (interactable) interactable.interactable = false;
        if (skillNameText) skillNameText.text = "Empty";
        if (descriptionText) descriptionText.text = "-";
        if (costText) costText.text = "-";
        if (icon) icon.overrideSprite = null;
    }
}
