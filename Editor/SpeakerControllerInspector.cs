using SpellBoundAR.DialogueSystem.Speakers;
using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    [CustomEditor(typeof(SpeakerController), true)]
    public class SpeakerControllerInspector : UnityEditor.Editor
    {
        private SpeakerController _speakerController;

        private void OnEnable()
        {
            _speakerController = (SpeakerController) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Start Conversation") && _speakerController) _speakerController.StartConversation();
        }
    }
}
