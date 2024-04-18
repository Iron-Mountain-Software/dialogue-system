using System;
using System.Collections.Generic;
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
            
            EditorGUILayout.Space(8);

            EditorGUILayout.BeginHorizontal(GUILayout.Height(25));

            if (GUILayout.Button("Add Basic Response", GUILayout.Height(25)))
            {
                AddNode(typeof(BasicDialogueResponseNode));
            }

            if (GUILayout.Button("Other", GUILayout.Height(25)))
            {
                RenderCreateResponseMenu();
            }
            
            EditorGUILayout.EndHorizontal();

            if (serializedObject.FindProperty("isTimed").boolValue && GUILayout.Button("Add Default Route", GUILayout.Height(25)))
            {
                DialogueLinesCreatorWindow.Open(_dialogueResponseBlockNode.GetPort("defaultResponse"));
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void RenderCreateResponseMenu()
        {
            if (TypeIndex.DialogueResponseNodeTypes.Count <= 0) return;
            GenericMenu menu = new GenericMenu();
            foreach (Type derivedType in TypeIndex.DialogueResponseNodeTypes)
            {
                if (derivedType == typeof(BasicDialogueResponseNode)) continue;
                menu.AddItem(new GUIContent(
                        "Add " + derivedType.Name),
                    false, 
                    () => AddNode(derivedType));
            }
            menu.ShowAsContext();
        }


        private void AddNode(Type type)
        {
            if (!ConversationEditor.Current || ConversationEditor.Current.graphEditor == null) return;
            Node node = ConversationEditor.Current.graphEditor.CreateNode(type, _dialogueResponseBlockNode.position);
            _dialogueResponseBlockNode.GetPort("responses").Connect(node.GetPort("input"));
            List<NodePort> responses = _dialogueResponseBlockNode.GetPort("responses").GetConnections();
            if (responses.Count <= 1)
            {
                int parentWidth = _dialogueResponseBlockNode.GetType()
                        .GetCustomAttributes(typeof(Node.NodeWidthAttribute), true).FirstOrDefault()
                    is Node.NodeWidthAttribute widthAttribute
                    ? widthAttribute.width
                    : 0;
                node.position = _dialogueResponseBlockNode.position
                                + Vector2.right * (parentWidth + 40);
            }
            else
            {
                NodePort nodePort = responses[^2];
                if (nodePort == null || !nodePort.node) return;
                node.position = nodePort.node.position + Vector2.down * -270;
            }
        }
    }
}
