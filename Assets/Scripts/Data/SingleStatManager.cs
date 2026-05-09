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
        if (member.Data.GetUnusedPoints() > 0) member.Data.usedStats[statName]++;
        manager.UpdateStatsValue();
    }

    public void LevelDown() {
        if (member.Data.usedStats[statName] > 5) member.Data.usedStats[statName]--;
        manager.UpdateStatsValue();
    }

}
