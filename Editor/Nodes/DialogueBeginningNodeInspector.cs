using System;
using IronMountain.DialogueSystem.Editor.Windows;
using IronMountain.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(DialogueBeginningNode))]
    public class DialogueBeginningNodeInspector : NodeEditor
    {
        private DialogueBeginningNode _dialogueBeginningNode;
        
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            if (!_dialogueBeginningNode) _dialogueBeginningNode = (DialogueBeginningNode) target;

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));

            
            foreach (NodePort dynamicPort in target.DynamicPorts)
            {
                if (!NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort))
                    NodeEditorGUILayout.PortField(dynamicPort, Array.Empty<GUILayoutOption>());
            }

            if (_dialogueBeginningNode.GetPort("output").ConnectionCount == 0
                && GUILayout.Button("Add Lines", GUILayout.Height(25)))
            {
                EditorGUILayout.Space(8);
                DialogueLinesCreatorWindow.Open(ConversationEditor.Current, _dialogueBeginningNode.GetPort("output"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
