using TMPro;

public class EnemyEncounterView : DataView<EnemyEncounter> {
    public TextMeshProUGUI encounterName;
    public ListView enemyList;

    public override void Subscribe() {
        encounterName.text = data.encounterName;
        if (enemyList) {
            enemyList.SetData(data.enemyList);
        }
    }

    public override void Unsubscribe() {
        if (enemyList) {
            enemyList.SetData(null);
        }
    }
}
