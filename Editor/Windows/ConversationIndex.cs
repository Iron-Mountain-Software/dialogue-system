using System;
using System.Collections.Generic;
using System.Text;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class ConversationIndex : EditorWindow
    {
        private static readonly Vector2 MinSize = new (300, 500);

        private ConversationListEditor _conversationListEditor;
        private UnityEditor.Editor _selectedConversationEditor;

        private Vector2 _sidebarScroll = Vector2.zero;
        
        private readonly List<Conversation> _conversations = new();

        public static ConversationIndex Open()
        {
            ConversationIndex window = GetWindow<ConversationIndex>(
                "Conversations", true, 
                typeof(NewConversationWindow), 
                typeof(ConversationIndex));
            window.minSize = MinSize;
            return window;
        }

        private void OnEnable()
        {
            _conversationListEditor = new ConversationListEditor();
            ConversationsManager.OnConversationsChanged += OnConversationsChanged;
        }

        private void OnFocus() => RefreshIndex();

        private void OnDisable()
        {
            ConversationsManager.OnConversationsChanged -= OnConversationsChanged;
        }

        private void OnConversationsChanged()
        {
            foreach (Conversation conversation in ConversationsManager.AllConversations)
            {
                if (!conversation || _conversations.Contains(conversation)) continue;
                _conversations.Add(conversation);
            }
        }

        private void RefreshIndex()
        {
            _conversations.Clear();
            AssetDatabase.Refresh();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Conversation)}");
            for ( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                Conversation conversation = AssetDatabase.LoadAssetAtPath<Conversation>( assetPath );
                if (!conversation || _conversations.Contains(conversation)) continue;
                _conversations.Add(conversation);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            DrawButtonMenu();
            DrawConversationList();
            EditorGUILayout.EndVertical();
        }

        private void DrawButtonMenu()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(40));
            if (GUILayout.Button("Create", GUILayout.ExpandHeight(true))) NewConversationWindow.Open();
            if (GUILayout.Button("Export", GUILayout.ExpandHeight(true))) ExportDialogueLines();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawConversationList()
        {
            _sidebarScroll.x = 0;
            _sidebarScroll = GUILayout.BeginScrollView(_sidebarScroll, false, false);
            _conversationListEditor.Draw(_conversations);
            GUILayout.EndScrollView();
        }

        private int CompareDialogueNodes(Node a, Node b)
        {
            if (!a) return -1;
            if (!b) return 1;
            return a.position.x.CompareTo(b.position.x);
        }

        private void ExportDialogueLines()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Conversation conversation in _conversations)
            {
                if (!conversation) continue;
                stringBuilder.Append("CONVERSATION: " + conversation.Name + "<br>");
                conversation.nodes.Sort(CompareDialogueNodes);
                foreach (Node node in conversation.nodes)
                {
                    if (!node) continue;
                    switch (node)
                    {
                        case DialogueLineWithAlternatesNode dialogueLineWithAlternatesNode:
                        {
                            string speaker = dialogueLineWithAlternatesNode.CustomSpeaker
                                ? dialogueLineWithAlternatesNode.CustomSpeaker.SpeakerName
                                : "Default";
                            Color color = dialogueLineWithAlternatesNode.CustomSpeaker
                                ? dialogueLineWithAlternatesNode.CustomSpeaker.Color
                                : Color.black;
                            string speakerFormatted = "<span style='color:" + ColorUtility.ToHtmlStringRGB(color) + "'><b>" + speaker + "</b></span>: ";
                            string mainText = dialogueLineWithAlternatesNode.Text;
                            stringBuilder.Append(speakerFormatted + mainText + "<br>");
                            foreach (DialogueLineMainContent content in dialogueLineWithAlternatesNode.AlternateContent)
                            {
                                stringBuilder.Append(speakerFormatted + content.Text + "<br>");
                            }
                            break;
                        }
                        case DialogueLineNode dialogueLineNode:
                        {
                            string speaker = dialogueLineNode.CustomSpeaker
                                ? dialogueLineNode.CustomSpeaker.SpeakerName
                                : "Default";
                            Color color = dialogueLineNode.CustomSpeaker
                                ? dialogueLineNode.CustomSpeaker.Color
                                : Color.black;
                            string speakerFormatted = "<span style='color:" + ColorUtility.ToHtmlStringRGB(color) + "'><b>" + speaker + "</b></span>: ";
                            stringBuilder.Append(speakerFormatted + dialogueLineNode.Text + "<br>");
                            break;
                        }
                    }
                }
                stringBuilder.AppendLine("<br>");
            }
            SaveSystem.SaveSystem.SaveFile("Exports", "Dialogue Lines.html", stringBuilder.ToString());
        }
    }
}