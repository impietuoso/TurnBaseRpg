using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EquipArrayAttribute), true)]
public class EquipArrayAttributeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var listProperty = property.FindPropertyRelative("list");

        var slots = Game.Config.equipmentOrder.Select(s => new GUIContent(s.ToString())).ToArray();
        var arrayMaxSize = slots.Length;
        if (listProperty.arraySize != arrayMaxSize)
            listProperty.arraySize = arrayMaxSize;

        EditorGUI.BeginProperty(position, label, property);

        var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);

        if (property.isExpanded) {
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            for (var i = 0; i < listProperty.arraySize; i++) {
                var elementProperty = listProperty.GetArrayElementAtIndex(i);
                var height = EditorGUI.GetPropertyHeight(elementProperty, true);
                rect.height = height;

                EditorGUI.PropertyField(rect, elementProperty, slots[i], true);
                rect.y += height + EditorGUIUtility.standardVerticalSpacing;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded) {
            var listProperty = property.FindPropertyRelative("list");
            if (listProperty != null) {
                for (var i = 0; i < listProperty.arraySize; i++) {
                    height += EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i), true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        return height;
    }
}