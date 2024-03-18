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
        private static string _lineSeparator = "\n";
        private static string _speakerSeparator = ":";
        private static int _conversationTypeIndex = 0;
        private static int _dialogueLineTypeIndex = 0;
        
        private string _folder = Path.Join("Scriptable Objects", "Conversations");
        private string _name = "New Conversation";
        private string _defaultInvokingLine = string.Empty;
        private string _lines = string.Empty;
        private float _spacing = 40f;

        private Conversation _conversation;
        private ConversationEditor _conversationEditor;
        
        public static void Open()
        {
            NewConversationWindow window = GetWindow(typeof(NewConversationWindow), false, "Create Conversation", true) as NewConversationWindow;
            window.minSize = new Vector2(500, 400);
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
            
            GUILayout.Label("Lines");
            _lines = GUILayout.TextArea(_lines, GUILayout.MinHeight(100));
            _lineSeparator = EditorGUILayout.TextField("Line Separator", _lineSeparator);
            _speakerSeparator = EditorGUILayout.TextField("Speaker Separator", _speakerSeparator);
            _dialogueLineTypeIndex = EditorGUILayout.Popup("Dialogue Line Type", _dialogueLineTypeIndex, TypeIndex.DialogueLineNodeTypeNames);
            _spacing = EditorGUILayout.FloatField("Spacing", _spacing);
            
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
            AssetDatabase.SaveAssets();

            _conversationEditor = ConversationEditor.Open(_conversation);
            
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            AddNodes();
            Close();
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

        private void AddNodes()
        {
            if (!_conversation)
            {
                Debug.Log("NULL conversation");
                return;
            }
            
            Vector2 lastSpawnedPosition = _conversationEditor.GridCenterPosition;
            Node lastSpawnedNode = AddNode(typeof(DialogueBeginningNode), ref lastSpawnedPosition);
            
            string[] lines = _lines.Split(_lineSeparator);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] segments = line.Split(_speakerSeparator, 2);
                Type dialogueLineType = TypeIndex.DialogueLineNodeTypes[_dialogueLineTypeIndex];
                Node node = AddNode(dialogueLineType, ref lastSpawnedPosition);
                if (node is DialogueLineNode dialogueLineNode)
                {
                    if (segments.Length > 1)
                    {
                        dialogueLineNode.CustomSpeaker = SpeakersIndexer.Find(segments[0].Trim());
                    }
                    dialogueLineNode.SimpleText = segments[^1].Trim();
                }
                if (lastSpawnedNode) lastSpawnedNode.GetPort("output").Connect(node.GetPort("input"));
                lastSpawnedNode = node;
            }
            
            Node endingNode = AddNode(typeof(DialogueEndingNode), ref lastSpawnedPosition);
            if (lastSpawnedNode) lastSpawnedNode.GetPort("output").Connect(endingNode.GetPort("input"));
        }

        private Node AddNode(Type type, ref Vector2 spawnPosition)
        {
            Node node = _conversationEditor && _conversationEditor.graphEditor != null 
                ? _conversationEditor.graphEditor.CreateNode(type, spawnPosition)
                : _conversation.AddNode(type);
            node.position = spawnPosition;

            if (type.GetCustomAttributes(typeof(Node.NodeWidthAttribute), true ).FirstOrDefault() is Node.NodeWidthAttribute widthAttribute)
            {
                spawnPosition.x += widthAttribute.width + _spacing;
            }
            return node;
        }
    }
}