using TricksAndTreatsOrThreats;
using UnityEditor;

[CustomEditor(typeof(CreatureSO))]
public class CreatureSOEditor : Editor {
    private readonly Stats _stats = new ();
    private bool _applyEquipment;

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Derived Stats (Preview)", EditorStyles.boldLabel);

        _applyEquipment = EditorGUILayout.Toggle("Apply Equipment Stats", _applyEquipment);
        if (EditorGUI.EndChangeCheck()) UpdateStats();

        EditorGUI.BeginDisabledGroup(true);
        foreach (var stat in Stats.All)
            EditorGUILayout.IntField(stat.ToString(), _stats[stat].Total);
        EditorGUI.EndDisabledGroup();
    }

    private void UpdateStats() {
        var creature = ((CreatureSO)target).Creature;
        if (creature == null) return;
        var stats = _applyEquipment ? creature : Stats.Zero;
        _stats.Recalculate(creature.level, creature, stats);
    }
}