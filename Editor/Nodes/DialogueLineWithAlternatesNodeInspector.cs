using IronMountain.DialogueSystem.Nodes;
using XNodeEditor;

namespace IronMountain.DialogueSystem.Editor.Nodes
{
    [CustomNodeEditor(typeof(DialogueLineWithAlternatesNode))]
    public class DialogueLineWithAlternatesNodeInspector : DialogueLineNodeInspector
    {
        public override void DrawAdditionalProperties()
        {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("alternateContent"));
        }
    }
}