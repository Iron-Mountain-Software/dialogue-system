using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Nodes.Conditions.Editor
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
