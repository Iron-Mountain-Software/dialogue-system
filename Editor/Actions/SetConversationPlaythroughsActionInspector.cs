using IronMountain.DialogueSystem.Actions;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor.Actions
{
    [CustomEditor(typeof(SetConversationPlaythroughsAction))]
    public class SetConversationPlaythroughsActionInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginHorizontal(); 
            EditorGUILayout.PropertyField(serializedObject.FindProperty("conversation"), GUIContent.none);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playthroughs"), GUIContent.none, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}