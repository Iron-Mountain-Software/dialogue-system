using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    public class ConversationsEditorWindow : EditorWindow
    {
        private static ConversationsEditorWindow Current { get; set; }

        private ConversationListEditor _conversationListEditor;
        private UnityEditor.Editor _selectedConversationEditor;
        
        private int _sidebarWidth = 450;
        
        private Rect _sidebarSection;
        private Rect _bodySection;
        
        private Vector2 _sidebarScroll = Vector2.zero;
        private Vector2 _contentScroll = Vector2.zero;
        
        private readonly List<Conversation> _conversations = new();

        public static void Open()
        {
            Current = GetWindow<ConversationsEditorWindow>("Conversations", true);
            Current.minSize = new Vector2(800, 700);
            Current.RefreshConversationsList();
        }
        
        private void RefreshConversationsList()
        {
            _conversations.Clear();
            AssetDatabase.Refresh();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Conversation)}");
            for( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                Conversation conversation = AssetDatabase.LoadAssetAtPath<Conversation>( assetPath );
                if (conversation) _conversations.Add(conversation);
            }
        }

        private void OnEnable()
        {
            _conversationListEditor = new ConversationListEditor();
        }

        private void OnGUI()
        {
            Current = this;
            DrawLayouts();
            
            GUILayout.BeginArea(_sidebarSection);
            DrawSidebar();
            GUILayout.EndArea();
            
            GUILayout.BeginArea(_bodySection);
            if (_conversationListEditor.SelectedConversation)
            {
                _contentScroll = GUILayout.BeginScrollView(_contentScroll);
                UnityEditor.Editor.CreateCachedEditor(_conversationListEditor.SelectedConversation, null, ref _selectedConversationEditor);
                _selectedConversationEditor.OnInspectorGUI();
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No conversation selected.");
                _selectedConversationEditor = null;
            }
            GUILayout.EndArea();
        }
        
        private void DrawLayouts()
        {
            _sidebarSection.x = 0;
            _sidebarSection.y = 0;
            _sidebarSection.width = _sidebarWidth;
            _sidebarSection.height = Current.position.height;
            
            _bodySection.x = _sidebarWidth;
            _bodySection.y = 0;
            _bodySection.width = Current.position.width - _sidebarWidth;
            _bodySection.height = Current.position.height;
        }
        
        private void DrawSidebar()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Refresh"))
            {
                RefreshConversationsList();
            }
            _sidebarScroll.x = 0;
            _sidebarScroll = GUILayout.BeginScrollView(_sidebarScroll, false, false);
            _conversationListEditor.Draw(_conversations);
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}