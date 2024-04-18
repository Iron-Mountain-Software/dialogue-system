using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class NewConversationWindow : EditorWindow
    {
        private static int _conversationTypeIndex = 0;

        private string _folder = Path.Join("Assets", "Scriptable Objects", "Conversations");
        private string _name = "New Conversation";
        private string _defaultInvokingLine = string.Empty;
        
        private ConversationEditor _conversationEditor;
        
        public static void Open()
        {
            NewConversationWindow window = GetWindow(typeof(NewConversationWindow), false, "Create Conversation", true) as NewConversationWindow;
            window.minSize = new Vector2(520, 160);
            window.maxSize = new Vector2(520, 160);
            window.wantsMouseMove = true;
        }

        protected void OnGUI()
        {
            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            _folder = EditorGUILayout.TextField("Folder: ", _folder);
            if (GUILayout.Button("Current", GUILayout.Width(60))) _folder = DirectoryUtilities.GetCurrentFolder();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            _name = EditorGUILayout.TextField("Name", _name);
            if (GUILayout.Button("Current", GUILayout.Width(60))) _name = GetCurrentName();
            EditorGUILayout.EndHorizontal();

            _conversationTypeIndex = EditorGUILayout.Popup("Type", _conversationTypeIndex, TypeIndex.ConversationTypeNames);
            _defaultInvokingLine = EditorGUILayout.TextField("Invoking Line", _defaultInvokingLine);
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Create Conversation", GUILayout.Height(35))) CreateConversation();
        }

        private string GetCurrentName()
        {
            string[] subfolders = _folder.Split(Path.DirectorySeparatorChar);
            return subfolders.Length > 0 ? subfolders[^1] : string.Empty;
        }

        private void CreateConversation()
        {
            Type conversationType = TypeIndex.ConversationTypes[_conversationTypeIndex];
            Conversation conversation = CreateInstance(conversationType) as Conversation;
            if (!conversation) return;
            DirectoryUtilities.CreateFolders(_folder);
            string path = Path.Combine(_folder, _name + ".asset");
            
            AssetDatabase.CreateAsset(conversation, path);
            conversation.DefaultInvokingLine = _defaultInvokingLine;
            EditorUtility.SetDirty(conversation);
            AssetDatabase.SaveAssetIfDirty(conversation);
            AssetDatabase.Refresh();

            Close();
            
            ConversationEditor.Open(conversation).Focus();
        }
    }
}