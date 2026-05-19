using System.Collections.Generic;
using TMPro;
using TricksAndTreatsOrThreats.UI;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public CreatureView targetView;
    public TextMeshProUGUI availablePointsText;
    public StatsView statsView;
    public List<SingleStatManager> stats;

    private void Awake() => statsView.SetData(new ());

    public void RouboUp() {
        if (targetView.Data.level < 20) targetView.Data.level++;
        UpdateStatsValue();
    }

    public void RouboDown() {
        if (targetView.Data.level > 1) targetView.Data.level--;
        UpdateStatsValue();
    }

    public void UpdateStatsValue() {
        var creature = targetView.Data;
        var hasPointsLeft = creature.GetUnusedPoints() > 0;
        foreach (var stat in stats) {
            stat.upButton.interactable = hasPointsLeft;
            stat.downButton.interactable = creature.LevelAttributes[stat.eStat] > 0;
            stat.atributeValueText.text = creature[stat.eStat].ToString();
        }

        statsView.Data.Recalculate(creature.level, creature, creature);
        availablePointsText.text = creature.GetUnusedPoints() + " points left";
    }
}