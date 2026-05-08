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
        if (interactable && !Data) interactable.interactable = false;
    }

    public string AllStats() {
        StringBuilder all = new StringBuilder();
        for (int i = 0; i < 5; i++) {
            var currentStat = (StatName)i;
            TrippleAppend(all, currentStat.ToString(), " - ", Data.usedStats[currentStat].ToString());
        }
        derivedStats.CalculateDeviredStats(Data.usedStats, Data.profession, Data.equips, Data.level);
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

    protected override void Subscribe() {
        if (nameText) nameText.text = Data.charName;
        if (levelText) levelText.text = $"Lv. {Data.level}";
        if (professionText) professionText.text = Data.profession.name;
        if (allStatsText) allStatsText.text = AllStats();
        if (equipedSkills) equipedSkills.SetData(Data.equipedSkills);
        if (avaliableSkills) avaliableSkills.SetData(Data.learnedSkills);
        if (equipments) {
            var sortedEquips = GameConfig.Instance.equipmentDrawOrder
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