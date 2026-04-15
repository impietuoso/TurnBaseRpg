using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public PartyMemberView memberView;
    public TextMeshProUGUI availablePointsText;
    public List<SingleStatManager> stats;
    public DerivedStats derivedStats;
    [Header("Derived Stats Text")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI evadeText;
    public TextMeshProUGUI resistenceText;

    public void RouboUp() {
        if(memberView.data.level < 20) memberView.data.level++;
        UpdateStatsValue();
    }

    public void RouboDown() {
        if(memberView.data.level > 1)memberView.data.level--;
        UpdateStatsValue();
    }

    public void UpdateDerivedStatsUI() {
        damageText.text = derivedStats.damage.currentValue.ToString();
        healthText.text = derivedStats.health.currentValue.ToString();
        manaText.text = derivedStats.mana.currentValue.ToString();
        speedText.text = derivedStats.speed.currentValue.ToString();
        evadeText.text = derivedStats.evade.currentValue.ToString();
        resistenceText.text = derivedStats.resistance.currentValue.ToString();
    }
    
    public void UpdateStatsValue() {
        var hasPointsLeft = memberView.data.GetUnusedPoints() > 0;
        foreach (var stat in stats) {
            stat.upButton.interactable = hasPointsLeft;
            
            if (memberView.data.usedStats[stat.statName] <= 5) stat.downButton.interactable = false;
            else stat.downButton.interactable = true;

            var characterStat = memberView.data.usedStats[stat.statName];
            var professionStats = memberView.data.profession.initialStats[stat.statName];
            stat.atributeValueText.text = (characterStat + professionStats).ToString();
        }

        var memberStats = memberView.data.usedStats;
        var memberProfession = memberView.data.profession;
        var memberEquips = memberView.data.equips;
        derivedStats.CalculateDeviredStats(memberStats, memberProfession, memberEquips, memberView.data.level);
        availablePointsText.text = memberView.data.GetUnusedPoints() + " points left";
        UpdateDerivedStatsUI();
    }
}