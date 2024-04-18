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

            if (GUILayout.Button("Add Response", GUILayout.ExpandHeight(true)))
            {
                RenderCreateResponseMenu();
            }

            if (serializedObject.FindProperty("isTimed").boolValue 
                && GUILayout.Button("Add Default Route", GUILayout.ExpandHeight(true)))
            {
                DialogueLinesCreatorWindow.Open(_dialogueResponseBlockNode.GetPort("defaultResponse"));
            }

            EditorGUILayout.EndHorizontal();


            serializedObject.ApplyModifiedProperties();
        }
        
        private void RenderCreateResponseMenu()
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
                        if (!ConversationEditor.Current || ConversationEditor.Current.graphEditor == null) return;
                        Node node = ConversationEditor.Current.graphEditor.CreateNode(derivedType, _dialogueResponseBlockNode.position);
                        _dialogueResponseBlockNode.GetPort("responses").Connect(node.GetPort("input"));
                        if (_dialogueResponseBlockNode.GetType().GetCustomAttributes(typeof(Node.NodeWidthAttribute), true ).FirstOrDefault() 
                                is Node.NodeWidthAttribute widthAttribute)
                        {
                            node.position = _dialogueResponseBlockNode.position + Vector2.right * (widthAttribute.width + 40);
                        }
                    });
            }
            menu.ShowAsContext();
        }
    }
}
