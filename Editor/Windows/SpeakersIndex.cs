using System.Collections.Generic;
using System.Text;
using IronMountain.DialogueSystem.Editor.Indexers;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Speakers;
using UnityEditor;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class SpeakersIndex : EditorWindow
    {
        private static readonly Vector2 MinSize = new (400, 400);
        private static readonly Vector2 MaxSize = new (600, 1200);
        
        private Vector2 _sidebarScroll = Vector2.zero;
        
        private readonly List<Conversation> _conversations = new();

        public static SpeakersIndex Open()
        {
            SpeakersIndex window = GetWindow<SpeakersIndex>(
                "Speakers", true,
                typeof(ConversationsIndex));
            window.minSize = MinSize;
            window.maxSize = MaxSize;
            return window;
        }

        private void OnFocus() => SpeakersIndexer.Refresh();

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            DrawButtonMenu();
            DrawSpeakersList();
            EditorGUILayout.EndVertical();
        }

        private void DrawButtonMenu()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(40));
            if (GUILayout.Button("Create", GUILayout.ExpandHeight(true))) NewSpeakerWindow.Open();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSpeakersList()
        {
            _sidebarScroll.x = 0;
            _sidebarScroll = GUILayout.BeginScrollView(_sidebarScroll, false, false);
            foreach (var speaker in SpeakersIndexer.Speakers)
            {
                if (!speaker) continue;
                if (GUILayout.Button(speaker.name, GUILayout.Height(30)))
                {
                    EditorGUIUtility.PingObject(speaker);
                }
            } 
            GUILayout.EndScrollView();
        }
    }
}