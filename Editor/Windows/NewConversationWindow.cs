using System;
using System.IO;
using System.Linq;
using IronMountain.DialogueSystem.Editor.Indexers;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class NewConversationWindow : EditorWindow
    {
        private static int _conversationTypeIndex = 0;

        private string _folder = Path.Join("Scriptable Objects", "Conversations");
        private string _name = "New Conversation";
        private string _defaultInvokingLine = string.Empty;
        
        private Conversation _conversation;
        private ConversationEditor _conversationEditor;
        
        public static void Open()
        {
            NewConversationWindow window = GetWindow(typeof(NewConversationWindow), false, "Create Conversation", true) as NewConversationWindow;
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);
            window.wantsMouseMove = true;
        }

        protected void OnGUI()
        {
            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Folder:  Assets" + Path.DirectorySeparatorChar, GUILayout.Width(90));
            _folder = EditorGUILayout.TextField(_folder);
            EditorGUILayout.EndHorizontal();
            _name = EditorGUILayout.TextField("Name", _name);
            _conversationTypeIndex = EditorGUILayout.Popup("Type", _conversationTypeIndex, TypeIndex.ConversationTypeNames);
            _defaultInvokingLine = EditorGUILayout.TextField("Invoking Line", _defaultInvokingLine);
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Create Conversation", GUILayout.Height(35))) CreateConversation();
        }

        private void CreateConversation()
        {
            Type conversationType = TypeIndex.ConversationTypes[_conversationTypeIndex];
            _conversation = CreateInstance(conversationType) as Conversation;
            CreateFolders();
            string path = Path.Combine("Assets", _folder, _name + ".asset");
            
            AssetDatabase.CreateAsset(_conversation, path);
            _conversation.DefaultInvokingLine = _defaultInvokingLine;
            EditorUtility.SetDirty(_conversation);
            AssetDatabase.SaveAssetIfDirty(_conversation);
            AssetDatabase.Refresh();

            Close();
            
            ConversationEditor.Open(_conversation).Focus();
        }

        private void CreateFolders()
        {
            string[] subfolders = _folder.Split(Path.DirectorySeparatorChar);
            string parent = "Assets";
            foreach (var subfolder in subfolders)
            {
                string child = Path.Join(parent, subfolder);
                if (!AssetDatabase.IsValidFolder(child)) AssetDatabase.CreateFolder(parent, subfolder);
                parent = child;
            }
        }
    }
}