using UnityEditor;
using UnityEngine;

namespace TricksAndTreatsOrThreats.Editor
{
    [CustomPropertyDrawer(typeof(ScoreT<>), true)]
    public class ScoreDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var key = property.FindPropertyRelative("key");
            var score = property.FindPropertyRelative("score");

            position.width /= 2;
            EditorGUI.PropertyField(position, key, GUIContent.none);

            position.x += position.width;
            EditorGUI.PropertyField(position, score, GUIContent.none);
        }

        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //     var score = property.FindPropertyRelative("score");
        //     const float scoreWidth = 155f;
        //     var keyRect = new Rect(position.x, position.y, position.width - scoreWidth, position.height);
        //     var scoreRect = new Rect(position.x + keyRect.width, position.y, scoreWidth, position.height);
        //     var scoreStyle = new GUIStyle(EditorStyles.label) { richText = true };
        //
        //     var scoreText = "";
        //     for (var i = 0; i <= TTT.ScoreCap; i++)
        //         scoreText += $"<color={(score.intValue == i ? "white" : "grey")}> {i % 10} </color>";
        //
        //     EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);
        //     EditorGUI.LabelField(scoreRect, scoreText, scoreStyle);
        //
        //     var e = Event.current;
        //     if (e.type == EventType.MouseDown && e.button == 1 && scoreRect.Contains(e.mousePosition))
        //     {
        //         var sectionWidth = scoreRect.width / (TTT.ScoreCap + 1);
        //         var clickedIndex = Mathf.FloorToInt((e.mousePosition.x - scoreRect.x) / sectionWidth);
        //         clickedIndex = Mathf.Clamp(clickedIndex, 0, TTT.ScoreCap);
        //
        //         score.intValue = clickedIndex;
        //         property.serializedObject.ApplyModifiedProperties();
        //
        //         e.Use();
        //     }
        // }
    }
}