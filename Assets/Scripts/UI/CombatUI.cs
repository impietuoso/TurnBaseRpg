using System;
using TMPro;
using UnityEngine;

[Obsolete]
public class CombatUI : MonoBehaviour {
    [Header("UI Components")]
    public GameObject currentActionPanel;
    public TextMeshProUGUI currentActionText;
    public StatusEffectListView statusEffectsDescription;

    public void ShowCurrentStatusEffects(StatusEffectListView statusEffectListView) {
        statusEffectsDescription.SetData(statusEffectListView.owner);
    }

    public void ShowCurrentAction(string userName, string actionName) {
        if (currentActionPanel.activeInHierarchy)
            currentActionPanel.SetActive(false);
        currentActionPanel.SetActive(true);
        currentActionText.text = userName + " uses " + actionName;
    }
}
