using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeDropdownAttribute), true)]
public class TypeDropdownAttributeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        var hasValue = property.managedReferenceValue != null;
        var typeName = hasValue ? property.managedReferenceValue.GetType().Name : "None (Select Effect)";
        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        var type = fieldInfo.FieldType;
        if (type.IsArray) type = type.GetElementType();
        else if (property.isArray) type = type.GetGenericArguments()[0];

        // Foldout / Selection Button
        if (GUI.Button(labelRect, new GUIContent(label.text + ": " + typeName), EditorStyles.popup)) {
            var iSkillEffect = TypeCache.GetTypesDerivedFrom(type);
            var myMenu = new GenericMenu();
            
            myMenu.AddItem(new GUIContent("None"), !hasValue, () => {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            foreach (var effectType in iSkillEffect) {
                myMenu.AddItem(
                    new GUIContent(effectType.Name),
                    hasValue && property.managedReferenceValue.GetType() == effectType,
                    () => {
                        property.managedReferenceValue = Activator.CreateInstance(effectType);
                        property.serializedObject.ApplyModifiedProperties();
                    });
            }
            myMenu.ShowAsContext();
        }

        // Draw child properties if value exists
        if (hasValue) {
            EditorGUI.indentLevel++;
            var fieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
            fieldRect.height = EditorGUI.GetPropertyHeight(property, true);

            EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var height = EditorGUIUtility.singleLineHeight;
        if (property.managedReferenceValue != null) 
            height += EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.standardVerticalSpacing;
        return height;
    }
}