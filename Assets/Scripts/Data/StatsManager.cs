using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public PartyMemberView memberView;
    public TextMeshProUGUI availablePointsText;
    public StatsView statsView;
    public List<SingleStatManager> stats;

    private void Awake() => statsView.SetData(new ());

    public void RouboUp() {
        if (memberView.Data.level < 20) memberView.Data.level++;
        UpdateStatsValue();
    }

    public void RouboDown() {
        if (memberView.Data.level > 1) memberView.Data.level--;
        UpdateStatsValue();
    }

    public void UpdateStatsValue() {
        var member = memberView.Data;
        var hasPointsLeft = member.GetUnusedPoints() > 0;
        foreach (var stat in stats) {
            stat.upButton.interactable = hasPointsLeft;
            stat.downButton.interactable = member.usedStats[stat.eStat] > 0;
            var characterStat = member.usedStats[stat.eStat];
            var professionStats = member.profession.InitialStats[stat.eStat];
            stat.atributeValueText.text = (characterStat + professionStats).ToString();
        }

        statsView.Data.Recalculate(member.level, member, member);
        availablePointsText.text = member.GetUnusedPoints() + " points left";
    }
}