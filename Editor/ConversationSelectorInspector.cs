using IronMountain.DialogueSystem.Selection;
using UnityEditor;
using UnityEngine;

namespace IronMountain.DialogueSystem.Editor
{
    [CustomEditor(typeof(ConversationSelector), true)]
    public class ConversationSelectorInspector : UnityEditor.Editor
    {
        private ConversationSelector _conversationSelector;

        private void OnEnable()
        {
            _conversationSelector = (ConversationSelector) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawRefreshButton();
        }

        private void DrawRefreshButton()
        {
            if (!GUILayout.Button("Refresh Next Conversation")) return;
            if (_conversationSelector) _conversationSelector.RefreshNextConversation();
        }
    }
}