using SpellBoundAR.DialogueSystem.Nodes;
using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueLineNode), true)]
    public class DialogueLineNodeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("TEST");
            base.OnInspectorGUI();
        }
    }
}
