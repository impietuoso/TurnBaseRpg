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
        if(memberView.Data.level < 20) memberView.Data.level++;
        UpdateStatsValue();
    }

    public void RouboDown() {
        if(memberView.Data.level > 1)memberView.Data.level--;
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
        var hasPointsLeft = memberView.Data.GetUnusedPoints() > 0;
        foreach (var stat in stats) {
            stat.upButton.interactable = hasPointsLeft;
            
            if (memberView.Data.usedStats[stat.statName] <= 5) stat.downButton.interactable = false;
            else stat.downButton.interactable = true;

            var characterStat = memberView.Data.usedStats[stat.statName];
            var professionStats = memberView.Data.profession.initialStats[stat.statName];
            stat.atributeValueText.text = (characterStat + professionStats).ToString();
        }

        var memberStats = memberView.Data.usedStats;
        var memberProfession = memberView.Data.profession;
        var memberEquips = memberView.Data.equips;
        derivedStats.CalculateDeviredStats(memberStats, memberProfession, memberEquips, memberView.Data.level);
        availablePointsText.text = memberView.Data.GetUnusedPoints() + " points left";
        UpdateDerivedStatsUI();
    }
}