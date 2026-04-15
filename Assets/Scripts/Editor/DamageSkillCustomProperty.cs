using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DamageSkill))]
public class DamageSkillCustomProperty : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        Rect drawRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Get properties
        SerializedProperty baseDamage = property.FindPropertyRelative("baseDamage");
        SerializedProperty ignoreShield = property.FindPropertyRelative("ignoreShield");
        SerializedProperty isPercentageDamage = property.FindPropertyRelative("isPercentageDamage");
        SerializedProperty healthPercentage = property.FindPropertyRelative("healthPercentage");
        SerializedProperty hitChance = property.FindPropertyRelative("hitChance");
        SerializedProperty criticalChance = property.FindPropertyRelative("criticalChance");
        SerializedProperty statMultiplier = property.FindPropertyRelative("statMultiplier");
        SerializedProperty damageStatScale = property.FindPropertyRelative("damageStatScale");
        SerializedProperty damageRange = property.FindPropertyRelative("damageRange");

        // Draw fields
        EditorGUI.PropertyField(drawRect, isPercentageDamage);
        drawRect.y += lineHeight;

        if (isPercentageDamage.boolValue) {
            EditorGUI.PropertyField(drawRect, healthPercentage);
            drawRect.y += lineHeight;
        } else {
            EditorGUI.PropertyField(drawRect, baseDamage);
            drawRect.y += lineHeight;
            EditorGUI.PropertyField(drawRect, statMultiplier);
            drawRect.y += lineHeight;
            EditorGUI.PropertyField(drawRect, damageStatScale);
            drawRect.y += lineHeight;
        }

        EditorGUI.PropertyField(drawRect, ignoreShield);
        drawRect.y += lineHeight;
        EditorGUI.PropertyField(drawRect, hitChance);
        drawRect.y += lineHeight;
        EditorGUI.PropertyField(drawRect, criticalChance);
        drawRect.y += lineHeight;
        EditorGUI.PropertyField(drawRect, damageRange);
        drawRect.y += lineHeight;

        // Draw damage range at the end as requested
        var minimalDamage = baseDamage.intValue * (1 - damageRange.floatValue);
        var maximalDamage = baseDamage.intValue * (1 + damageRange.floatValue);
        EditorGUI.LabelField(drawRect, "Final Damage Range: " + minimalDamage + " ~ " + maximalDamage);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        SerializedProperty isPercentage = property.FindPropertyRelative("isPercentageDamage");
        int lineCount = isPercentage.boolValue ? 7 : 9;
        return (lineCount * EditorGUIUtility.singleLineHeight) + (lineCount * EditorGUIUtility.standardVerticalSpacing);
    }
}
