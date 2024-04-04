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
        private readonly GUIContent _speechBubbleContent = new ("SP", "Speech Bubble Preview");
        private readonly GUIContent _thoughtBubbleContent = new ("TH", "Thought Bubble Preview");
        private readonly GUIContent _loopingEnabledContent = new ("↺", "Looping enabled");

        private Dictionary<Conversation, bool> _drawers = new ();
        
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

            if (!_drawers.ContainsKey(conversation)) _drawers.Add(conversation, false);
            
            if (conversation.HasErrors())
            {
                EditorGUILayout.BeginHorizontal(_redBox, GUILayout.MaxHeight(35), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = new Color(1,1,1, .5f);
                if (GUILayout.Button(EditorGUIUtility.IconContent("Error"), GUILayout.Width(25), GUILayout.ExpandHeight(true)))
                {
                    _drawers[conversation] = !_drawers[conversation];
                }
            }
            else if (conversation.HasWarnings())
            {
                EditorGUILayout.BeginHorizontal(_yellowBox, GUILayout.MaxHeight(35), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = new Color(1,1,1, .5f);
                if (GUILayout.Button(EditorGUIUtility.IconContent("Warning"), GUILayout.Width(25), GUILayout.ExpandHeight(true)))
                {
                    _drawers[conversation] = !_drawers[conversation];
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal(_grayBox, GUILayout.MaxHeight(35), GUILayout.ExpandHeight(false));
                GUILayout.Label(EditorGUIUtility.IconContent("TestPassed"), GUILayout.Width(25), GUILayout.ExpandHeight(true));
                _drawers[conversation] = false;
            }

            EditorGUI.BeginDisabledGroup(conversation == SelectedConversation);
            if (GUILayout.Button(conversation.name, GUILayout.ExpandHeight(true)))
            {
                ConversationEditor.Open(conversation);
                SelectedConversation = conversation;
            }
            EditorGUI.EndDisabledGroup();

            GUI.backgroundColor = Color.white;

            DrawPreviewBox(conversation.PreviewType);
            DrawLoopingBox(conversation.Looping);
            DrawAutoplayBox(conversation.BehaviorWhenEnqueued);
            DrawPriorityBox(conversation.Priority);
            DrawStateBox(conversation.IsActive);

            EditorGUILayout.EndHorizontal();

            if (_drawers[conversation])
            {
                foreach (string warning in conversation.Warnings)
                {
                    GUILayout.Label("Warning: " + warning);
                }
                foreach (string error in conversation.Errors)
                {
                    GUILayout.Label("Error: " + error);
                }
            }
        }

        private void DrawPriorityBox(int priority)
        {
            GUILayout.Label(priority.ToString(), _grayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
        }
        
        private void DrawPreviewBox(ConversationPreviewType previewType)
        {
            switch (previewType)
            {
                case ConversationPreviewType.SpeechBubble:
                    GUILayout.Label(_speechBubbleContent, _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
                    break;
                case ConversationPreviewType.ThoughtBubble:
                    GUILayout.Label(_thoughtBubbleContent, _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
                    break;
            }
        }
        
        private void DrawLoopingBox(bool looping)
        {
            if (looping) GUILayout.Label(_loopingEnabledContent, _grayBox, GUILayout.Width(25), GUILayout.ExpandHeight(true));
        }
        
        private void DrawAutoplayBox(Conversation.BehaviorWhenQueued behaviorWhenQueued)
        {
            switch (behaviorWhenQueued)
            {
                case Conversation.BehaviorWhenQueued.Played:
                    GUILayout.Label(_autoPlayEnabledContent, _grayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
            }
        }

        private void DrawStateBox(bool active)
        {
            GUILayout.Label(active ? "Active" : "Inactive", active ? _greenBox : _grayBox, GUILayout.Width(70), GUILayout.ExpandHeight(true));
        }
    }
}
