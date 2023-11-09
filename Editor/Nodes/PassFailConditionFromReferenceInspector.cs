using IronMountain.Conditions.Editor;
using IronMountain.DialogueSystem.Nodes.Conditions;
using UnityEditor;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(PassFailConditionFromReference))]
    public class PassFailConditionFromReferenceInspector : NodeEditor
    {
        private PassFailConditionFromReference _passFailConditionFromReference;
        private ConditionEditor _conditionEditor;
        
        public override void OnCreate()
        {
            base.OnCreate();
            _passFailConditionFromReference = (PassFailConditionFromReference) target;
            _conditionEditor = new ConditionEditor("Condition", _passFailConditionFromReference.graph, newCondition => _passFailConditionFromReference.condition = newCondition);
        }

        public virtual void DrawAdditionalProperties() { }

        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));
            EditorGUILayout.BeginVertical();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("pass"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("fail"));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            _conditionEditor.Draw(ref _passFailConditionFromReference.condition);
            
            // Iterate through dynamic ports and draw them in the order in which they are serialized
            foreach (XNode.NodePort dynamicPort in target.DynamicPorts) {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
