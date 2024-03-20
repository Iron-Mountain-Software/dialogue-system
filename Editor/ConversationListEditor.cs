using System;
using System.Collections.Generic;
using IronMountain.DialogueSystem.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor
{
    public class ConversationListEditor
    {
        public event Action OnSelectedConversationChanged;
        
        private Conversation _selectedConversation;

        public Conversation SelectedConversation
        {
            get => _selectedConversation;
            set
            {
                if (_selectedConversation == value) return;
                _selectedConversation = value;
                OnSelectedConversationChanged?.Invoke();
            }
        }

        private readonly GUIStyle _grayBox;
        private readonly GUIStyle _redBox;
        private readonly GUIStyle _yellowBox;
        private readonly GUIStyle _greenBox;

        private readonly GUIContent _autoPlayEnabledContent = new ("▶", "Autoplay enabled");
        private readonly GUIContent _autoPlayDisabledContent = new (string.Empty, "Autoplay disabled");
        
        private readonly GUIContent _loopingEnabledContent = new ("↺", "Looping enabled");
        private readonly GUIContent _loopingDisabledContent = new (string.Empty, "Looping disabled");

        public ConversationListEditor()
        {
            Texture2D grayTexture = new Texture2D(1, 1);
            grayTexture.SetPixel(0,0, new Color(0f, 0f, 0f, 0.4f));
            grayTexture.Apply();
            _grayBox = new GUIStyle {
                margin = new RectOffset(2, 2, 1, 1),
                padding = new RectOffset(2, 2, 2, 2),
                alignment = TextAnchor.MiddleCenter,
                normal = {textColor = Color.white, background = grayTexture}
            };
            
            Texture2D redTexture = new Texture2D(1, 1);
            redTexture.SetPixel(0,0, new Color(0.53f, 0.08f, 0f));
            redTexture.Apply();
            _redBox = new GUIStyle {
                margin = new RectOffset(2, 2, 1, 1),
                padding = new RectOffset(2, 2, 2, 2),
                alignment = TextAnchor.MiddleCenter,
                normal = {textColor = Color.white, background = redTexture}
            };
            
            Texture2D yellowTexture = new Texture2D(1, 1);
            yellowTexture.SetPixel(0,0, new Color(0.8f, 0.62f, 0f));
            yellowTexture.Apply();
            _yellowBox = new GUIStyle {
                margin = new RectOffset(2, 2, 1, 1),
                padding = new RectOffset(2, 2, 2, 2),
                alignment = TextAnchor.MiddleCenter,
                normal = {textColor = Color.white, background = yellowTexture}
            };
            
            Texture2D greenTexture = new Texture2D(1, 1);
            greenTexture.SetPixel(0,0, new Color(0.09f, 0.78f, 0.11f));
            greenTexture.Apply();
            _greenBox = new GUIStyle {
                margin = new RectOffset(2, 2, 1, 1),
                alignment = TextAnchor.MiddleCenter,
                normal = {textColor = Color.white, background = greenTexture}
            };
        }

        public void Draw(List<Conversation> conversations)
        {
            foreach (Conversation conversation in conversations)
            {
                if (conversation) DrawEntry(conversation);
            }
        }
        
        private void DrawEntry(Conversation conversation)
        {
            if (!conversation) return;

            if (conversation.HasErrors())
            {
                EditorGUILayout.BeginHorizontal(_redBox, GUILayout.MaxHeight(25), GUILayout.ExpandHeight(false));
                GUILayout.Label(EditorGUIUtility.IconContent("Error"), GUILayout.Width(25), GUILayout.ExpandHeight(true));
            }
            else if (conversation.HasWarnings())
            {
                EditorGUILayout.BeginHorizontal(_yellowBox, GUILayout.MaxHeight(25), GUILayout.ExpandHeight(false));
                GUILayout.Label(EditorGUIUtility.IconContent("Warning"), GUILayout.Width(25), GUILayout.ExpandHeight(true));
            }
            else
            {
                EditorGUILayout.BeginHorizontal(_grayBox, GUILayout.MaxHeight(25), GUILayout.ExpandHeight(false));
                GUILayout.Label(EditorGUIUtility.IconContent("TestPassed"), GUILayout.Width(25), GUILayout.ExpandHeight(true));
            }

            EditorGUI.BeginDisabledGroup(conversation == SelectedConversation);
            if (GUILayout.Button(conversation.name))
            {
                ConversationEditor.Open(conversation);
                SelectedConversation = conversation;
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Label(conversation.Priority.ToString(), _grayBox, GUILayout.MinWidth(20), GUILayout.MaxWidth(20), GUILayout.ExpandHeight(true));
            
            switch (conversation.BehaviorWhenEnqueued)
            {
                case Conversation.BehaviorWhenQueued.None:
                    GUILayout.Label(_autoPlayDisabledContent, _grayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
                case Conversation.BehaviorWhenQueued.Played:
                    GUILayout.Label(_autoPlayEnabledContent, _grayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
            }
            
            switch (conversation.PreviewType)
            {
                case ConversationPreviewType.SpeechBubble:
                    GUILayout.Label("Sp", _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
                    break;
                case ConversationPreviewType.ThoughtBubble:
                    GUILayout.Label("Th", _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
                    break;
                case ConversationPreviewType.None:
                    GUILayout.Label("", _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
                    break;
            }

            GUILayout.Label(conversation.Looping ? _loopingEnabledContent : _loopingDisabledContent, _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
            
            bool active = conversation.IsActive;
            GUILayout.Label(active ? "Active" : "Inactive", active ? _greenBox : _grayBox, GUILayout.Width(70), GUILayout.ExpandHeight(true));

            EditorGUILayout.EndHorizontal();
        }
    }
}
