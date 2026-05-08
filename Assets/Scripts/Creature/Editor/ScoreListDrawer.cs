using UnityEditor;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Editor
{
    [CustomPropertyDrawer(typeof(ScoreList<>), true)]
    public class ScoreListDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property.FindPropertyRelative("values"));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("values"), label);
        }
    }
}