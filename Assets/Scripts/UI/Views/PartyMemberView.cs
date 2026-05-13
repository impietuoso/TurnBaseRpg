using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberView : DataView<PartyMember> {
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text professionText;
    public TMP_Text allStatsText;
    public Image icon;
    public ListView equipedSkills;
    public ListView avaliableSkills;
    public ListView equipments;
    public CanvasGroup interactable;
    private Stats _stats = new ();

    private void Start() {
        if (interactable && !Data) interactable.interactable = false;
    }

    private string AllStats() {
        var all = new StringBuilder();
        for (var i = 0; i < 5; i++) {
            var currentStat = (Attribute)i;
            TrippleAppend(all, currentStat.ToString(), " - ", Data.usedStats[currentStat].ToString());
        }
        foreach (var stat in Stats.All)
            TrippleAppend(all, stat.ToString(), " - ", _stats[stat].ToString());

        return all.ToString();
    }

    private void TrippleAppend(StringBuilder all, string a, string b, string c) {
        all.Append(a);
        all.Append(b);
        all.AppendLine(c);
    }

    protected override void Subscribe() {
        if (nameText) nameText.text = Data.charName;
        if (levelText) levelText.text = $"Lv. {Data.level}";
        if (professionText) professionText.text = Data.profession.name;
        if (allStatsText) allStatsText.text = AllStats();
        if (equipedSkills) equipedSkills.SetData(Data.equipedSkills);
        if (avaliableSkills) avaliableSkills.SetData(Data.learnedSkills);
        if (equipments) {
            var sortedEquips = Game.Config.equipmentDrawOrder
                .Select(type => Data.equips[type]);
            equipments.SetData(sortedEquips);
        }
        if (icon) icon.overrideSprite = Data.uiSprite;
        if (interactable) interactable.interactable = true;
    }

    protected override void Unsubscribe() {
        if (interactable) interactable.interactable = false;
        if (nameText) nameText.text = "Empty";
        if (levelText) levelText.text = "";
        if (professionText) professionText.text = "-";
        if (icon) icon.overrideSprite = null;
    }
}