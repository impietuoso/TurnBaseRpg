using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberView : DataView<PartyMember> {
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI professionText;
    public TextMeshProUGUI allStatsText;
    public Image icon;
    public ListView equipedSkills;
    public ListView avaliableSkills;
    public ListView equipments;
    public CanvasGroup interactable;
    private DerivedStats derivedStats = new();

    private void Start() {
        if (interactable && !data) interactable.interactable = false;
    }

    public string AllStats() {
        StringBuilder all = new StringBuilder();
        for (int i = 0; i < 5; i++) {
            var currentStat = (StatName)i;
            TrippleAppend(all, currentStat.ToString(), " - ", data.usedStats[currentStat].ToString());
        }
        derivedStats.CalculateDeviredStats(data.usedStats, data.profession, data.equips, data.level);
        TrippleAppend(all, "Damage", " - ", derivedStats.damage.currentValue.ToString());
        TrippleAppend(all, "Health", " - ", derivedStats.health.currentValue.ToString());
        TrippleAppend(all, "Mana", " - ", derivedStats.mana.currentValue.ToString());
        TrippleAppend(all, "Speed", " - ", derivedStats.speed.currentValue.ToString());
        TrippleAppend(all, "Evade", " - ", derivedStats.evade.currentValue.ToString());
        TrippleAppend(all, "Resistance", " - ", derivedStats.resistance.currentValue.ToString());

        return all.ToString();
    }

    public void TrippleAppend(StringBuilder all, string a, string b, string c) {
        all.Append(a);
        all.Append(b);
        all.AppendLine(c);
    }

    public override void Subscribe() {
        if (nameText) nameText.text = data.charName;
        if (levelText) levelText.text = $"Lv. {data.level}";
        if (professionText) professionText.text = data.profession.name;
        if (allStatsText) allStatsText.text = AllStats();
        if (equipedSkills) equipedSkills.SetData(data.equipedSkills);
        if (avaliableSkills) avaliableSkills.SetData(data.learnedSkills);
        if (equipments) {
            var sortedEquips = GameConfig.Instance.equipmentDrawOrder
                .Select(type => data.equips[type]);
            equipments.SetData(sortedEquips);
        }
        if (icon) icon.overrideSprite = data.uiSprite;
        if (interactable) interactable.interactable = true;
    }

    public override void Unsubscribe() {
        if (interactable) interactable.interactable = false;
        if (nameText) nameText.text = "Empty";
        if (levelText) levelText.text = "";
        if (professionText) professionText.text = "-";
        if (icon) icon.overrideSprite = null;
    }
}