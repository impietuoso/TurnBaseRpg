using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListTemplateView : MonoBehaviour {
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI extraText;

    public void UpdateTemplateUI(Skill skill) {
        if(icon) icon.sprite = skill.icon;
        if(nameText) nameText.text = skill.skillName;
        if(extraText) extraText.text = skill.cost + "\n <size=14>MP</size>";
    }
    
    public void UpdateTemplateUI(Status status) {
        if(icon) icon.sprite = status.source.statusIcon;
        if(nameText) nameText.text = status.statusName;
        if(extraText) extraText.text = status.statusDescription;
    }
}