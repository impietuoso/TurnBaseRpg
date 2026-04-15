using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObservableList<Equipment>), true)]
public class ObsercableListDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var listProperty = property.FindPropertyRelative("list");
        EditorGUI.PropertyField(position, listProperty, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var listProperty = property.FindPropertyRelative("list");
        return EditorGUI.GetPropertyHeight(listProperty, label, true);
    }
}
