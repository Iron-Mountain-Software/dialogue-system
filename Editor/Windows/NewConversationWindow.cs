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
        
        private Conversation _conversation;
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
            if (GUILayout.Button("Current", GUILayout.Width(60))) _folder = GetCurrentFolder();
            EditorGUILayout.EndHorizontal();
            _name = EditorGUILayout.TextField("Name", _name);
            _conversationTypeIndex = EditorGUILayout.Popup("Type", _conversationTypeIndex, TypeIndex.ConversationTypeNames);
            _defaultInvokingLine = EditorGUILayout.TextField("Invoking Line", _defaultInvokingLine);
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Create Conversation", GUILayout.Height(35))) CreateConversation();
        }

        private string GetCurrentFolder()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            return getActiveFolderPath is not null 
                ? getActiveFolderPath.Invoke(null, new object[0]).ToString()
                : string.Empty;
        }

        private void CreateConversation()
        {
            Type conversationType = TypeIndex.ConversationTypes[_conversationTypeIndex];
            _conversation = CreateInstance(conversationType) as Conversation;
            CreateFolders();
            string path = Path.Combine(_folder, _name + ".asset");
            
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
            if (subfolders.Length == 0) return;
            string parent = subfolders[0];
            for (int index = 1; index < subfolders.Length; index++)
            {
                var subfolder = subfolders[index];
                string child = Path.Join(parent, subfolder);
                if (!AssetDatabase.IsValidFolder(child)) AssetDatabase.CreateFolder(parent, subfolder);
                parent = child;
            }
        }
    }
}