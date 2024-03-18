using System;
using System.Linq;
using IronMountain.DialogueSystem.Editor.Indexers;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;

namespace IronMountain.DialogueSystem.Editor.Windows
{
    public class DialogueLinesCreatorWindow : EditorWindow
    {
        private static string _lineSeparator = "\n";
        private static string _speakerSeparator = ":";
        private static int _dialogueLineTypeIndex = 0;

        private ConversationEditor _conversationEditor;

        private string _lines = string.Empty;
        private float _spacing = 40f;
        
        public static void Open(ConversationEditor conversationEditor)
        {
            DialogueLinesCreatorWindow window = GetWindow(typeof(DialogueLinesCreatorWindow), false, "Add Lines", true) as DialogueLinesCreatorWindow;
            window.minSize = new Vector2(500, 400);
            window.wantsMouseMove = true;
            window._conversationEditor = conversationEditor;
        }
        
        protected void OnGUI()
        {
            _lines = GUILayout.TextArea(_lines, GUILayout.MinHeight(100));
            _lineSeparator = EditorGUILayout.TextField("Line Separator", _lineSeparator);
            _speakerSeparator = EditorGUILayout.TextField("Speaker Separator", _speakerSeparator);
            _dialogueLineTypeIndex = EditorGUILayout.Popup("Dialogue Line Type", _dialogueLineTypeIndex, TypeIndex.DialogueLineNodeTypeNames);
            _spacing = EditorGUILayout.FloatField("Spacing", _spacing);
            if (GUILayout.Button("Create!", GUILayout.Height(35))) CreateNodes();
        }

        private void CreateNodes()
        {
            Node lastSpawnedNode = null;
            Vector2 lastSpawnedPosition = _conversationEditor.GridCenterPosition;

            string[] lines = _lines.Split(_lineSeparator);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] segments = line.Split(_speakerSeparator, 2);
                Type dialogueLineType = TypeIndex.DialogueLineNodeTypes[_dialogueLineTypeIndex];
                Node node = _conversationEditor.graphEditor.CreateNode(dialogueLineType, lastSpawnedPosition);
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
                if (dialogueLineType.GetCustomAttributes(typeof(Node.NodeWidthAttribute), true ).FirstOrDefault() is Node.NodeWidthAttribute widthAttribute)
                {
                    lastSpawnedPosition.x += widthAttribute.width + _spacing;
                }
            }
            
            Close();
        }
    }
}