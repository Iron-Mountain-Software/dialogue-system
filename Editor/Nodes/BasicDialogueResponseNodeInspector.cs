using System;
using System.Linq;
using IronMountain.DialogueSystem.Editor.Windows;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Nodes.ResponseGenerators;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(BasicDialogueResponseNode))]
    public class BasicDialogueResponseNodeInspector : NodeEditor
    {
        private BasicDialogueResponseNode _dialogueResponseNode;

        public virtual void DrawAdditionalProperties() { }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            if (!_dialogueResponseNode) _dialogueResponseNode = (BasicDialogueResponseNode) target;

            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!DialogueLineNodeInspector.Localize);
            if (GUILayout.Button("Simple")) DialogueLineNodeInspector.Localize = false;
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(DialogueLineNodeInspector.Localize);
            if (GUILayout.Button("Localized")) DialogueLineNodeInspector.Localize = true;
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (DialogueLineNodeInspector.Localize)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"));
            }
            else
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("stringText"), GUIContent.none);
            }

            EditorGUILayout.Space(10);
            
            string[] strArray = new string[8]
            {
                "m_Script",
                "graph",
                "position",
                "ports",
                "input",
                "output",
                "text",
                "stringText"
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

            if (_dialogueResponseNode.GetPort("output").ConnectionCount == 0
                && GUILayout.Button("Add Lines", GUILayout.Height(25)))
            {
                EditorGUILayout.Space(8);
                DialogueLinesCreatorWindow.Open(_dialogueResponseNode.GetPort("output"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
