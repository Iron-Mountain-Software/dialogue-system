using SpellBoundAR.DialogueSystem.Editor.Nodes;
using SpellBoundAR.DialogueSystem.Nodes;
using XNodeEditor;

namespace SpellBoundAR.DialogueSystem.Editor
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