using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DamageSkill))]
public class DamageSkillCustomProperty : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (property.isExpanded) {
            // Don't make child fields be indented relative to the foldout, but we usually indent the group
            EditorGUI.indentLevel++;
            
            var lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var drawRect = new Rect(position.x, position.y + lineHeight, position.width, EditorGUIUtility.singleLineHeight);

            // Get properties
            var baseDamage = property.FindPropertyRelative("baseDamage");
            var ignoreShield = property.FindPropertyRelative("ignoreShield");
            var isPercentageDamage = property.FindPropertyRelative("isPercentageDamage");
            var healthPercentage = property.FindPropertyRelative("healthPercentage");
            var hitChance = property.FindPropertyRelative("hitChance");
            var criticalChance = property.FindPropertyRelative("criticalChance");
            var statMultiplier = property.FindPropertyRelative("statMultiplier");
            var damageStatScale = property.FindPropertyRelative("damageStatScale");
            var damageRange = property.FindPropertyRelative("damageRange");

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

            // Draw damage range label
            if (!isPercentageDamage.boolValue) {
                var minimalDamage = baseDamage.intValue * (1 - damageRange.floatValue);
                var maximalDamage = baseDamage.intValue * (1 + damageRange.floatValue);
                EditorGUI.LabelField(drawRect, "Final Damage Range: " + minimalDamage.ToString("F1") + " ~ " + maximalDamage.ToString("F1"));
            } else {
                var minimalDamage = healthPercentage.floatValue * (1 - damageRange.floatValue);
                var maximalDamage = healthPercentage.floatValue * (1 + damageRange.floatValue);
                EditorGUI.LabelField(drawRect, "Final % Range: " + (minimalDamage * 100).ToString("F1") + "% ~ " + (maximalDamage * 100).ToString("F1") + "%");
            }
            
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (!property.isExpanded) {
            return EditorGUIUtility.singleLineHeight;
        }

        var isPercentage = property.FindPropertyRelative("isPercentageDamage");
        // Foldout(1) + isPercentage(1) + (health(1) OR base+mult+scale(3)) + ignore+hit+crit+range(4) + finalLabel(1)
        var lineCount = isPercentage.boolValue ? 8 : 10;
        return (lineCount * EditorGUIUtility.singleLineHeight) + (lineCount * EditorGUIUtility.standardVerticalSpacing);
    }
}
