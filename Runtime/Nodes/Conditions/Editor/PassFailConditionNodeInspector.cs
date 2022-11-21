using UnityEditor;
using UnityEngine;

namespace ARISE.DialogueSystem.Nodes.Conditions.Editor
{
    [CustomEditor(typeof(PassFailCondition), true)]
    public class PassFailConditionNodeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Test"))
            {
                Debug.Log("woohoo");
            }

            DrawDefaultInspector();
        }
    }
}
