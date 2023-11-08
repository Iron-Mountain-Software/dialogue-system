using IronMountain.Conditions;
using IronMountain.Conditions.Editor;
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

        private GUIStyle _validContainer;
        private GUIStyle _invalidContainer;
        private GUIStyle _header;
        
        private void OnEnable()
        {
            _conversation = (Conversation) target;
            
            Texture2D validContainerTexture = new Texture2D(1, 1);
            validContainerTexture.SetPixel(0,0, new Color(0.2f, 0.2f, 0.2f));
            validContainerTexture.Apply();
            _validContainer = new GUIStyle {
                padding = new RectOffset(7,7,7,7),
                normal = { background = validContainerTexture }
            };
            
            Texture2D invalidContainerTexture = new Texture2D(1, 1);
            invalidContainerTexture.SetPixel(0,0, new Color(0.41f, 0f, 0.04f));
            invalidContainerTexture.Apply();
            _invalidContainer = new GUIStyle {
                padding = new RectOffset(7,7,7,7),
                normal = { background = invalidContainerTexture }
            };
            
            _header = new GUIStyle
            {
                alignment = TextAnchor.LowerLeft,
                fontSize = 15,
                padding = new RectOffset(2,2,2,2),
                fontStyle = FontStyle.Bold,
                normal = {textColor = new Color(0.45f, 0.45f, 0.45f)}
            };
        }

        public override void OnInspectorGUI()
        {
            //if (GUILayout.Button("Log Dialogue Interaction Content", GUILayout.MinHeight(30)))
                //Debug.Log(DialogueInteractionPrinter.PrintDialogueInteraction((Conversation)target));
            EditorGUILayout.Space();
            DrawGeneralSection();
            DrawPrioritySection();
            DrawPreviewSection();
            DrawConditionSection(_conversation);
            DrawPlaybackSection();
            DrawOtherProperties();
            DrawStateSections();
            GUILayout.Space(10);
            DrawSelectThisButton();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawSelectThisButton()
        {
            if (GUILayout.Button("Select this conversation", GUILayout.MinHeight(30)))
                UnityEditor.Selection.activeObject = target;
        }

        protected virtual void DrawGeneralSection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_conversation.GeneralSectionHasErrors ? _invalidContainer : _validContainer, GUILayout.MinHeight(75));
            GUILayout.Label("General", _header, GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID", GUILayout.MaxWidth(100));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("id"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPrioritySection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_conversation.PrioritySectionHasErrors ? _invalidContainer : _validContainer, GUILayout.MinHeight(75));
            GUILayout.Label("Priority", _header, GUILayout.ExpandWidth(true));
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
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPreviewSection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_conversation.PreviewHasErrors ? _invalidContainer : _validContainer, GUILayout.MinHeight(75));
            GUILayout.Label("Preview", _header, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("alertInConversationMenu"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("previewType"), GUIContent.none);
            if (serializedObject.FindProperty("previewType").enumValueFlag != (int) ConversationPreviewType.None)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("previewText"), GUIContent.none);
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawConditionSection(Conversation conversation)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_conversation.ConditionHasErrors ? _invalidContainer : _validContainer, GUILayout.MinHeight(75));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Condition", _header);
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
                AddConditionMenu.Open(conversation, "Condition", newCondition =>
                {
                    conversation.Condition = newCondition;
                });
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("condition"), GUIContent.none, false);
            if (conversation.Condition)
            {
                CreateCachedEditor(conversation.Condition, null, ref _cachedConditionsEditor);
                _cachedConditionsEditor.OnInspectorGUI();
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPlaybackSection()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_validContainer, GUILayout.MinHeight(75));
            EditorGUILayout.LabelField("Playback", _header);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("behaviorWhenQueued"), false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("looping"), false);
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawOtherProperties()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_validContainer, GUILayout.MinHeight(75));
            EditorGUILayout.LabelField("Other", _header);
            DrawPropertiesExcluding(serializedObject,
                "m_Script",
                "nodes",
                "id",
                "prioritizeOverDefault",
                "priority",
                "invokingLine",
                "invokingIcon",
                "alertInConversationMenu",
                "previewType",
                "previewText",
                "condition",
                "behaviorWhenQueued",
                "looping"
            );
            EditorGUILayout.EndVertical();
            
        }

        protected virtual void DrawStateSections()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(_validContainer, GUILayout.MinHeight(75));
            EditorGUILayout.LabelField("Saved Data", _header);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Active", GUILayout.MaxWidth(100));
            EditorGUILayout.LabelField(_conversation.IsActive ? "TRUE" : "FALSE");
            if (GUILayout.Button("Refresh", GUILayout.MaxWidth(75)) && _conversation) _conversation.RefreshActiveState();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Playthroughs", GUILayout.MaxWidth(100));
            EditorGUILayout.LabelField(_conversation.Playthroughs.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}
