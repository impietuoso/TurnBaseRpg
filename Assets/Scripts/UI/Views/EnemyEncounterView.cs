using TMPro;

public class EnemyEncounterView : DataView<EnemyEncounter> {
    public TextMeshProUGUI encounterName;
    public ListView enemyList;

    protected override void Subscribe() {
        encounterName.text = Data.encounterName;
        if (enemyList) {
            enemyList.SetData(Data.enemyList);
        }
    }

    protected override void Unsubscribe() {
        if (enemyList) {
            enemyList.SetData(null);
        }
    }
}
