using IronMountain.Conditions.Editor;
using IronMountain.DialogueSystem.Conditions;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor.Conditions
{
    [CustomEditor(typeof(ConditionConversationPlaythroughs))]
    public class ConditionConversationPlaythroughsInspector : ConditionInspector
    {
        protected override void DrawProperties()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("conversation"), GUIContent.none);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("comparison"), GUIContent.none, GUILayout.Width(38));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playthroughs"), GUIContent.none, GUILayout.Width(20));
            EditorGUILayout.LabelField("PLAYS", GUILayout.Width(45));
            EditorGUILayout.EndHorizontal();
        }
    }
}
