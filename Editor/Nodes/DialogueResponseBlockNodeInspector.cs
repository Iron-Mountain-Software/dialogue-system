using System;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(DialogueResponseBlockNode))]
    public class DialogueResponseBlockNodeInspector : NodeEditor
    {
        private DialogueResponseBlockNode _dialogueResponseBlockNode;
        
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            if (!_dialogueResponseBlockNode) _dialogueResponseBlockNode = (DialogueResponseBlockNode) target;

            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"), true, GUILayout.Width(60));
            EditorGUILayout.BeginVertical();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("responses"));
            if (serializedObject.FindProperty("isTimed").boolValue)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("defaultResponse"), new GUIContent("(Default)"));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("isTimed"));
            
            if (serializedObject.FindProperty("isTimed").boolValue)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("seconds"));
            }
            
            foreach (NodePort dynamicPort in target.DynamicPorts)
            {
                if (!NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort))
                    NodeEditorGUILayout.PortField(dynamicPort, Array.Empty<GUILayoutOption>());
            }
            
            if (GUILayout.Button("Add Response"))
            {
                RenderCreateMenu();
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void RenderCreateMenu()
        {
            if (TypeIndex.DialogueResponseNodeTypes.Count <= 0) return;
            GenericMenu menu = new GenericMenu();
            foreach (Type derivedType in TypeIndex.DialogueResponseNodeTypes)
            {
                menu.AddItem(new GUIContent(
                        "Add " + derivedType.Name),
                    false,
                    () =>
                    {
                        Node node = _dialogueResponseBlockNode.graph.AddNode(derivedType);
                        node.position = _dialogueResponseBlockNode.position;
                    });
            }
            menu.ShowAsContext();
        }
    }
}
