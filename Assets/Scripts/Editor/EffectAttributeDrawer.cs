using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeDropdownAttribute), true)]
public class TypeDropdownAttributeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        bool hasValue = property.managedReferenceValue != null;
        string typeName = hasValue ? property.managedReferenceValue.GetType().Name : "None (Select Effect)";

        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        // Foldout / Selection Button
        if (GUI.Button(labelRect, new GUIContent(label.text + ": " + typeName), EditorStyles.popup)) {
            var attributeType = (attribute as TypeDropdownAttribute).type;
            var iSkillEffect = TypeCache.GetTypesDerivedFrom(attributeType);
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
            Rect fieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
            fieldRect.height = EditorGUI.GetPropertyHeight(property, true);

            EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);

            //SerializedProperty iterator = property.Copy();
            //SerializedProperty endProperty = iterator.GetEndProperty();

            /*if (iterator.NextVisible(true)) {
                do {
                    if (SerializedProperty.EqualContents(iterator, endProperty)) break;

                    float height = EditorGUI.GetPropertyHeight(iterator, true);
                    fieldRect.height = height;
                    EditorGUI.PropertyField(fieldRect, iterator, true);
                    fieldRect.y += height + EditorGUIUtility.standardVerticalSpacing;
                } while (iterator.NextVisible(false));
            }*/
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float height = EditorGUIUtility.singleLineHeight;

        height += EditorGUI.GetPropertyHeight(property, label, true);
        
        /*if (property.managedReferenceValue != null) {
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();

            if (iterator.NextVisible(true)) {
                do {
                    if (SerializedProperty.EqualContents(iterator, endProperty)) break;
                    height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
                } while (iterator.NextVisible(false));
            }
        }*/

        return height;
    }
}