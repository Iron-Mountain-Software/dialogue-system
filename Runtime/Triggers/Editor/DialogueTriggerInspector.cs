using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Triggers.Editor
{
    [CustomEditor(typeof(DialogueTrigger), true)]
    public class DialogueTriggerInspector : UnityEditor.Editor
    {
        private DialogueTrigger _dialogueTrigger;

        private void OnEnable()
        {
            _dialogueTrigger = (DialogueTrigger) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawTriggerButton();
        }

        private void DrawTriggerButton()
        {
            if (!GUILayout.Button("Trigger")) return;
            if (_dialogueTrigger) _dialogueTrigger.TriggerConversation();
        }
    }
}
