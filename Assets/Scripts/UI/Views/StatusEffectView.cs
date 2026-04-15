using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectView : MonoBehaviour {
    public Image statusIcon;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    private Status owner;

    public void SetInfo(Status status) {
        owner = status;
        statusIcon.sprite = status.source.statusIcon;
        if (durationText) durationText.text = status.DisplayValue.Value.ToString();
        if (nameText) nameText.text = status.statusName;
        if (descriptionText) descriptionText.text = status.statusDescription;
        status.DisplayValue.OnChange += UpdateUI;
    }

    private void OnDestroy() {
        if(owner!= null) owner.DisplayValue.OnChange -= UpdateUI;
    }

    public void UpdateUI(int i) {
        if(durationText) durationText.text = i.ToString();
    }
}
