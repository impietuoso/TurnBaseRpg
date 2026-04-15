using System.Linq;
using UnityEditor;

[CustomEditor(typeof(PartyMember))]
public class PartyMemberEditor : Editor {
    private DerivedStats publicStats = new();
    public bool applyEquipmentStats;
    
    public void OnEnable() {
        var partyMember = (PartyMember)target;
        publicStats.CalculateDeviredStats(
            partyMember.usedStats, partyMember.profession, partyMember.equips, partyMember.level);
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Derived Stats (Preview)", EditorStyles.boldLabel);

        applyEquipmentStats = EditorGUILayout.Toggle("Apply Equipment Stats", applyEquipmentStats);
        var change = EditorGUI.EndChangeCheck();
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.IntField("Damage", publicStats.damage?.currentValue ?? 0);
        EditorGUILayout.IntField("Health", publicStats.health?.maxValue ?? 0);
        EditorGUILayout.IntField("Mana", publicStats.mana?.maxValue ?? 0);
        EditorGUILayout.IntField("Shield", publicStats.shield?.maxValue ?? 0);
        EditorGUILayout.IntField("Speed", publicStats.speed?.currentValue ?? 0);
        EditorGUILayout.IntField("Armor", publicStats.armor?.currentValue ?? 0);
        EditorGUILayout.IntField("Resistance", publicStats.resistance?.currentValue ?? 0);
        EditorGUILayout.IntField("Evade", publicStats.evade?.currentValue ?? 0);
        EditorGUI.EndDisabledGroup();
        
        if (change) {
            var partyMember = (PartyMember)target;

            var partyMemberEquips = applyEquipmentStats ? partyMember.equips : null;
            
            publicStats.CalculateDeviredStats(
                partyMember.usedStats, partyMember.profession, partyMemberEquips, partyMember.level);
        }
    }
}