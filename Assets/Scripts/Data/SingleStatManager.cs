using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleStatManager : MonoBehaviour {
    public PartyMemberView member;
    public StatsManager manager;
    public StatName statName;

    [Header("UI")]
    public TextMeshProUGUI atributeValueText;
    public Button upButton;
    public Button downButton;

    public void LevelUp() {
        if (member.data.GetUnusedPoints() > 0) member.data.usedStats[statName]++;
        manager.UpdateStatsValue();
    }

    public void LevelDown() {
        if (member.data.usedStats[statName] > 5) member.data.usedStats[statName]--;
        manager.UpdateStatsValue();
    }

}
