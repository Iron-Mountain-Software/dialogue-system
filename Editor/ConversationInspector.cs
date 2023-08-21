using SpellBoundAR.Conditions;
using SpellBoundAR.Conditions.Editor;
using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    [CustomEditor(typeof(Conversation), true)]
    public class ConversationInspector : UnityEditor.Editor
    {
        [Header("Cache")]
        private static UnityEditor.Editor _cachedConditionsEditor;
        private Conversation _conversation;

        private void OnEnable()
        {
            _conversation = (Conversation) target;
        }

        protected virtual void DrawMoreInGeneralSection() { }
        protected virtual void DrawAdditionalSections() { }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Select", GUILayout.MinHeight(30))) 
                UnityEditor.Selection.activeObject = target;
            //if (GUILayout.Button("Log Dialogue Interaction Content", GUILayout.MinHeight(30)))
                //Debug.Log(DialogueInteractionPrinter.PrintDialogueInteraction((Conversation)target));
            EditorGUILayout.Space();
            DrawGeneralSection();
            DrawPrioritySection();
            DrawPreviewSection();
            DrawConditionSection(_conversation);
            DrawPlaybackSection();
            DrawAdditionalSections();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawGeneralSection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(Styles.Container, GUILayout.MinHeight(75));
            GUILayout.Label("General", Styles.Header, GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID", GUILayout.MaxWidth(20));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("id"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            DrawMoreInGeneralSection();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPrioritySection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(Styles.Container, GUILayout.MinHeight(75));
            GUILayout.Label("Priority", Styles.Header, GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prioritize");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("prioritizeOverDefault"), GUIContent.none,
                GUILayout.MaxWidth(75));
            EditorGUILayout.EndHorizontal();
            if (serializedObject.FindProperty("prioritizeOverDefault").boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty priority = serializedObject.FindProperty("priority");
                GUILayout.Label("Priority");
                if (GUILayout.Button("←", GUILayout.MaxWidth(20))) priority.intValue--;
                EditorGUILayout.PropertyField(priority, GUIContent.none, GUILayout.MaxWidth(50));
                if (GUILayout.Button("→", GUILayout.MaxWidth(20))) priority.intValue++;
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("invokingLine"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("invokingIcon"), GUIContent.none);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPreviewSection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(Styles.Container, GUILayout.MinHeight(75));
            GUILayout.Label("Preview", Styles.Header, GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("alertInConversationMenu"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("previewType"), GUIContent.none);
            if (serializedObject.FindProperty("previewType").enumValueFlag != (int) ConversationPreviewType.None)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("previewText"), GUIContent.none);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawConditionSection(Conversation conversation)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(Styles.Container, GUILayout.MinHeight(75));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Condition", Styles.Header);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50));
            if (conversation.Condition && GUILayout.Button("Remove"))
            {
                Condition condition = conversation.Condition;
                if (!condition || string.IsNullOrEmpty(AssetDatabase.GetAssetPath(condition))) return;
                AssetDatabase.RemoveObjectFromAsset(condition);
                conversation.Condition = null;
                DestroyImmediate(condition);
                AssetDatabase.SaveAssets();
            }
            else if (!conversation.Condition && GUILayout.Button("Add"))
            {
                AddConditionMenu addConditionMenu = new AddConditionMenu(conversation, "Condition");
                addConditionMenu.OnConditionCreated += (condition) => conversation.Condition = condition;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("condition"), GUIContent.none, false);
            if (conversation.Condition)
            {
                CreateCachedEditor(conversation.Condition, null, ref _cachedConditionsEditor);
                _cachedConditionsEditor.OnInspectorGUI();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPlaybackSection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(Styles.Container, GUILayout.MinHeight(75));
            EditorGUILayout.LabelField("Playback", Styles.Header);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("behaviorWhenQueued"), false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("looping"), false);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}
