using IronMountain.DialogueSystem.Nodes.Actions;
using IronMountain.ScriptableActions.Editor;
using UnityEditor;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(ScriptableActionNode))]
    public class ScriptableActionNodeInspector : NodeEditor
    {
        private ScriptableActionNode _scriptableActionNode;
        private ScriptableActionsEditor _actionsEditor;
        
        public override void OnCreate()
        {
            base.OnCreate();
            _scriptableActionNode = (ScriptableActionNode) target;
            _actionsEditor = new ScriptableActionsEditor("Actions", _scriptableActionNode.graph, _scriptableActionNode.actions);
        }
        
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("output"));
            EditorGUILayout.EndHorizontal();

            // Iterate through dynamic ports and draw them in the order in which they are serialized
            foreach (XNode.NodePort dynamicPort in target.DynamicPorts) {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
            
            _actionsEditor.Draw();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
