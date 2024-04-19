using IronMountain.DialogueSystem.UI;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor
{
    [CustomEditor(typeof(ConversationPlayer), true)]
    public class ConversationPlayerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("conversation"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnStart"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isMuted"));
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Auto Advance", GUILayout.Width(190));
            SerializedProperty autoAdvance = serializedObject.FindProperty("autoAdvance");
            EditorGUILayout.PropertyField(autoAdvance, GUIContent.none, GUILayout.Width(25));
            EditorGUI.BeginDisabledGroup(!autoAdvance.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoAdvanceSeconds"), GUIContent.none, GUILayout.Width(50));
            EditorGUILayout.LabelField("seconds");
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Self Destruct", GUILayout.Width(190));
            SerializedProperty selfDestruct = serializedObject.FindProperty("selfDestruct");
            EditorGUILayout.PropertyField(selfDestruct, GUIContent.none, GUILayout.Width(25));
            EditorGUI.BeginDisabledGroup(!selfDestruct.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("destructionDelay"), GUIContent.none, GUILayout.Width(50));
            EditorGUILayout.LabelField("seconds");
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("responseBlockParent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("responseBlockPrefab"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
