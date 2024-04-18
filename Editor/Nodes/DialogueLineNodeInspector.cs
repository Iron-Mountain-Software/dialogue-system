using System;
using System.Linq;
using IronMountain.DialogueSystem.Editor.Windows;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(DialogueLineNode))]
    public class DialogueLineNodeInspector : NodeEditor
    {
        public static bool Localize = false;
        
        private DialogueLineNode _dialogueLineNode;

        public virtual void DrawAdditionalProperties() { }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            if (!_dialogueLineNode) _dialogueLineNode = (DialogueLineNode) target;

            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
            EditorGUILayout.EndHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("customSpeaker"));

            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!Localize);
            if (GUILayout.Button("Simple")) Localize = false;
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(Localize);
            if (GUILayout.Button("Localized")) Localize = true;
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (Localize)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("localizedAudio"), new GUIContent("Narration"));
            }
            else
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("simpleText"), GUIContent.none);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Narration", GUILayout.Width(55));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"), GUIContent.none);
                EditorGUI.BeginDisabledGroup(!_dialogueLineNode.AudioClip);
                if (GUILayout.Button("Rename", GUILayout.Width(60)))
                {
                    string newName = _dialogueLineNode.Text;
                    newName = newName.Replace("/", "");
                    newName = newName.Replace("?", "");
                    newName = newName.Replace("<", "");
                    newName = newName.Replace(">", "");
                    newName = newName.Replace("\\", "");
                    newName = newName.Replace(":", "");
                    newName = newName.Replace("*", "");
                    newName = newName.Replace("|", "");
                    newName = newName.Replace("\"", "");
                    if (string.IsNullOrWhiteSpace(newName)) return;
                    _dialogueLineNode.AudioClip.name = newName;
                    string path = AssetDatabase.GetAssetPath(_dialogueLineNode.AudioClip);
                    AssetDatabase.RenameAsset(path, newName);
                    EditorUtility.SetDirty(_dialogueLineNode.AudioClip);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space(10);
            
            string[] strArray = new string[12]
            {
                "m_Script",
                "graph",
                "position",
                "ports",
                "input",
                "output",
                "speakerType",
                "customSpeaker",
                "text",
                "localizedAudio",
                "simpleText",
                "audioClip"
            };
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (!strArray.Contains(iterator.name))
                    NodeEditorGUILayout.PropertyField(iterator, true, Array.Empty<GUILayoutOption>());
            }
            
            foreach (NodePort dynamicPort in target.DynamicPorts)
            {
                if (!NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort))
                    NodeEditorGUILayout.PortField(dynamicPort, Array.Empty<GUILayoutOption>());
            }

            if (_dialogueLineNode.GetPort("output").ConnectionCount == 0)
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.BeginHorizontal(GUILayout.Height(25));
                if (GUILayout.Button("Add Lines", GUILayout.ExpandHeight(true))) DialogueLinesCreatorWindow.Open(_dialogueLineNode.GetPort("output"));
                if (GUILayout.Button("Add Responses", GUILayout.ExpandHeight(true))) AddNode<DialogueResponseBlockNode>();
                if (GUILayout.Button("Add Ending", GUILayout.ExpandHeight(true))) AddNode<DialogueEndingNode>();
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddNode<T>() where T : DialogueNode
        {
            if (!ConversationEditor.Current || ConversationEditor.Current.graphEditor == null) return;
            Node node = ConversationEditor.Current.graphEditor.CreateNode(typeof(T), _dialogueLineNode.position);
            _dialogueLineNode.GetPort("output").Connect(node.GetPort("input"));
            if (_dialogueLineNode.GetType().GetCustomAttributes(typeof(Node.NodeWidthAttribute), true ).FirstOrDefault() 
                is Node.NodeWidthAttribute widthAttribute)
            {
                node.position = _dialogueLineNode.position + Vector2.right * (widthAttribute.width + 40);
            }
        }
    }
}
