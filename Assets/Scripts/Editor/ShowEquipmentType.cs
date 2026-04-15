using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowEquipmentTypeAtribute), true)]
public class ShowEquipmentType : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {        
        var listProperty = property.FindPropertyRelative("list");
        
        var arrayMaxSize = GameConfig.Instance.equipmentArrayOrder.Length;
        if (listProperty.arraySize != arrayMaxSize) {
            listProperty.arraySize = arrayMaxSize;
        }

        EditorGUI.BeginProperty(position, label, property);

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);
        
        if (property.isExpanded) {
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            for (int i = 0; i < listProperty.arraySize; i++) {
                var elementProperty = listProperty.GetArrayElementAtIndex(i);
                var betterLabel = GameConfig.Instance.equipmentArrayOrder[i].typeName;
                
                float height = EditorGUI.GetPropertyHeight(elementProperty, true);
                rect.height = height;
                
                EditorGUI.PropertyField(rect, elementProperty, new GUIContent(betterLabel), true);
                
                rect.y += height + EditorGUIUtility.standardVerticalSpacing;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded) {
            var listProperty = property.FindPropertyRelative("list");
            if (listProperty != null) {
                for (int i = 0; i < listProperty.arraySize; i++) {
                    height += EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i), true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        return height;
    }
}
