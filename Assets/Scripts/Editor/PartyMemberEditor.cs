using UnityEditor;

[CustomEditor(typeof(PartyMember))]
public class PartyMemberEditor : Editor {
    private readonly Stats _stats = new ();
    private bool _applyEquipment;

    public void OnEnable() => UpdateStats();

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
        var partyMember = (PartyMember)target;
        var stats = _applyEquipment ? partyMember : Stats.Zero;
        _stats.Recalculate(partyMember.level, partyMember, stats);
    }
}