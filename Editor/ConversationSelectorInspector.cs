using IronMountain.DialogueSystem.Editor.Indexers;
using IronMountain.DialogueSystem.Selection;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor
{
    [CustomEditor(typeof(ConversationSelector), true)]
    public class ConversationSelectorInspector : UnityEditor.Editor
    {
        private ConversationSelector _conversationSelector;
        private bool _disableDeepEdit = true;

        private void OnEnable()
        {
            _conversationSelector = (ConversationSelector) target;
        }

        public override void OnInspectorGUI()
        {
            if (!_conversationSelector) return;
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nextConversation"));
            if (GUILayout.Button("Refresh", GUILayout.Width(70))) _conversationSelector.RefreshNextConversation();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            SerializedProperty speakerProperty = serializedObject.FindProperty("speaker");
            EditorGUILayout.PropertyField(speakerProperty);
            SerializedObject serializedSpeaker = speakerProperty != null && speakerProperty.objectReferenceValue != null
                ? new SerializedObject(speakerProperty.objectReferenceValue)
                : null;
            if (serializedSpeaker != null)
            {
                if (_disableDeepEdit && GUILayout.Button("Edit", GUILayout.Width(70))) _disableDeepEdit = false;
                else if (!_disableDeepEdit && GUILayout.Button("Done", GUILayout.Width(70))) _disableDeepEdit = true;
            }
            else if (GUILayout.Button("Set", GUILayout.Width(70)))
            {
                GenericMenu menu = new GenericMenu();
                foreach (var speaker in SpeakersIndexer.Speakers)
                {
                    menu.AddItem(new GUIContent(speaker.SpeakerName), false, () =>
                    {
                        _disableDeepEdit = true;
                        if (speakerProperty != null) speakerProperty.objectReferenceValue = speaker;
                        if (serializedObject != null) serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            }

            EditorGUILayout.EndHorizontal();
            if (serializedSpeaker != null)
            {
                EditorGUI.BeginDisabledGroup(_disableDeepEdit);
                EditorGUILayout.PropertyField(serializedSpeaker.FindProperty("defaultConversation"));
                EditorGUILayout.PropertyField(serializedSpeaker.FindProperty("conversations"), new GUIContent("Speaker Conversations"));
                serializedSpeaker.ApplyModifiedProperties();
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("additionalConversations"), 
                serializedSpeaker == null
                    ? new GUIContent("Conversations")
                    : new GUIContent("Additional Conversations"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}