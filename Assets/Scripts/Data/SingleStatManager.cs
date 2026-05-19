using TMPro;
using TricksAndTreatsOrThreats.UI;
using UnityEngine;
using UnityEngine.UI;

public class SingleStatManager : MonoBehaviour {
    public CreatureView targetView;
    public StatsManager manager;
    public Attribute eStat;

    [Header("UI")]
    public TextMeshProUGUI atributeValueText;
    public Button upButton;
    public Button downButton;

    public void LevelUp() {
        if (targetView.Data.GetUnusedPoints() > 0) targetView.Data.LevelAttributes[eStat]++;
        manager.UpdateStatsValue();
    }

    public void LevelDown() {
        if (targetView.Data.LevelAttributes[eStat] > 5) targetView.Data.LevelAttributes[eStat]--;
        manager.UpdateStatsValue();
    }

}
