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
        private enum LineSeparatorType
        {
            Newline,
            Tab,
            Custom
        }

        private static LineSeparatorType _lineLineSeparatorType = LineSeparatorType.Newline;
        private static string _lineSeparator = "\n";
        private static string _speakerSeparator = ":";
        private static int _dialogueLineTypeIndex = 0;

        private static string LineSeparator
        {
            get
            {
                switch (_lineLineSeparatorType)
                {
                    case LineSeparatorType.Newline:
                        return "\n";
                    case LineSeparatorType.Tab:
                        return "\t";
                    default:
                        return _lineSeparator;
                }
            }
        }

        private NodePort _previousOutput = null;
        private Vector2 _linesInputScroll = Vector2.zero;
        private string _lines = string.Empty;
        private float _spacing = 40f;
        
        public static void Open(NodePort previousOutput = null)
        {
            DialogueLinesCreatorWindow window = GetWindow(typeof(DialogueLinesCreatorWindow), false, "Add Lines", true) as DialogueLinesCreatorWindow;
            window.minSize = new Vector2(500, 400);
            window.wantsMouseMove = true;
            window._previousOutput = previousOutput;
        }
        
        protected void OnGUI()
        {
            _linesInputScroll = EditorGUILayout.BeginScrollView(_linesInputScroll, GUILayout.ExpandWidth(true), GUILayout.MinHeight(position.height - 130));
            _linesInputScroll.x = 0;
            _lines = GUILayout.TextArea(_lines);
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            _lineLineSeparatorType = (LineSeparatorType) EditorGUILayout.EnumPopup("Line Separator", _lineLineSeparatorType);
            EditorGUI.BeginDisabledGroup(_lineLineSeparatorType != LineSeparatorType.Custom);
            _lineSeparator = EditorGUILayout.TextField(_lineSeparator);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            _speakerSeparator = EditorGUILayout.TextField("Speaker Separator", _speakerSeparator);
            _dialogueLineTypeIndex = EditorGUILayout.Popup("Dialogue Line Type", _dialogueLineTypeIndex, TypeIndex.DialogueLineNodeTypeNames);
            _spacing = EditorGUILayout.FloatField("Spacing", _spacing);
            if (GUILayout.Button("Create!", GUILayout.Height(35))) CreateNodes();
        }

        private void CreateNodes()
        {
            if (!ConversationEditor.Current || ConversationEditor.Current.graphEditor == null) return;
            string[] lines = _lines.Split(LineSeparator);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] segments = line.Split(_speakerSeparator, 2);
                Type dialogueLineType = TypeIndex.DialogueLineNodeTypes[_dialogueLineTypeIndex];
                Node node = ConversationEditor.Current.graphEditor.CreateNode(dialogueLineType, GetSpawnPosition());
                if (node is DialogueLineNode dialogueLineNode)
                {
                    if (segments.Length > 1)
                    {
                        dialogueLineNode.CustomSpeaker = SpeakersIndexer.Find(segments[0].Trim());
                    }
                    dialogueLineNode.SimpleText = segments[^1].Trim();
                }
                _previousOutput?.Connect(node.GetPort("input"));
                _previousOutput = node.GetPort("output");
            }
            
            Close();
        }

        private Vector2 GetSpawnPosition()
        {
            if (_previousOutput != null
                && _previousOutput.node
                && _previousOutput.node.GetType().GetCustomAttributes(typeof(Node.NodeWidthAttribute), true ).FirstOrDefault() 
                    is Node.NodeWidthAttribute widthAttribute)
            {
                return new Vector2(_previousOutput.node.position.x + widthAttribute.width + _spacing, _previousOutput.node.position.y);
            }

            return ConversationEditor.Current 
                ? ConversationEditor.Current.GridCenterPosition 
                : Vector2.zero;
        }
    }
}