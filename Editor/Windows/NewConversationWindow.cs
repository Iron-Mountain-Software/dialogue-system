using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Speakers;
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
        private string _defaultInvokingLine;
        private string _lines;
        private float _spacing = 40f;

        private Conversation _conversation;
        private ConversationEditorWindow _conversationEditor;

        private readonly List<Speaker> _speakers = new();

        public static void Open()
        {
            NewConversationWindow window = GetWindow(typeof(NewConversationWindow), false, "Create Conversation", true) as NewConversationWindow;
            window.minSize = new Vector2(500, 400);
            window.wantsMouseMove = true;
            window.GetAllSpeakers();
        }
        
        private void GetAllSpeakers()
        {
            _speakers.Clear();
            AssetDatabase.Refresh();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Speaker)}");
            for( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                Speaker speaker = AssetDatabase.LoadAssetAtPath<Speaker>( assetPath );
                if (speaker) _speakers.Add(speaker);
            }
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
            
            if (GUILayout.Button("Create!", GUILayout.Height(35))) CreateConversation();

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

            _conversationEditor = ConversationEditorWindow.Open(_conversation);
            
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            AddNodes();
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

            //_conversation.AddNode<DialogueBeginningNode>();
            
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
                        dialogueLineNode.CustomSpeaker = GetSpeaker(segments[0].Trim());
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
            Node node = _conversationEditor.graphEditor.CreateNode(type, spawnPosition);
            if (type.GetCustomAttributes(typeof(Node.NodeWidthAttribute), true ).FirstOrDefault() is Node.NodeWidthAttribute widthAttribute)
            {
                spawnPosition.x += widthAttribute.width + _spacing;
            }
            return node;
        }

        private Speaker GetSpeaker(string query)
        {
            return _speakers.FirstOrDefault(speaker => speaker && ((ISpeaker) speaker).UsesNameOrAlias(query));
        }
    }
}