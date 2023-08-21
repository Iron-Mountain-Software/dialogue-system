using UnityEditor;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    public static class ConversationsEditor
    {
        public static Conversation DrawDialogueInteractionList(Conversation selectedConversation)
        {
            foreach (Conversation conversation in Database.Instance.Conversations.list)
            {
                if (!conversation) continue;
                if (DrawDialogueInteractionRow(conversation, selectedConversation))
                {
                    selectedConversation = conversation;
                }
            }
            return selectedConversation;
        }
        
        public static bool DrawDialogueInteractionRow(Conversation conversation, Conversation selectedConversation = null)
        {
            bool selected = false;
            if (!conversation) return selected;
            EditorGUILayout.BeginHorizontal();
            
            bool active = conversation.Condition && conversation.Condition.Evaluate();
            GUILayout.Label(active ? "On" : "Off", active ? Styles.GreenBox : Styles.GrayBox, GUILayout.MinWidth(30), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));

            EditorGUI.BeginDisabledGroup(conversation == selectedConversation);
            if (GUILayout.Button(conversation.name)) selected = true;
            EditorGUI.EndDisabledGroup();

            GUILayout.Label(conversation.Priority.ToString(), Styles.GrayBox, GUILayout.MinWidth(20), GUILayout.MaxWidth(20), GUILayout.ExpandHeight(true));
            
            switch (conversation.BehaviorWhenEnqueued)
            {
                case Conversation.BehaviorWhenQueued.None:
                    GUILayout.Label("", Styles.GrayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
                case Conversation.BehaviorWhenQueued.Played:
                    GUILayout.Label("▶", Styles.GrayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
            }
            
            switch (conversation.PreviewType)
            {
                case ConversationPreviewType.SpeechBubble:
                    GUILayout.Label("Sp", Styles.GrayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
                case ConversationPreviewType.ThoughtBubble:
                    GUILayout.Label("Th", Styles.GrayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
                case ConversationPreviewType.None:
                    GUILayout.Label("", Styles.GrayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
                    break;
            }
            
            GUILayout.Label(conversation.Looping ? "↺" : "", Styles.GrayBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));

            if (conversation.HasErrors()) GUILayout.Label("✗", Styles.RedBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
            else if (conversation.HasWarnings()) GUILayout.Label("⚠", Styles.YellowBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
            else GUILayout.Label("✓", Styles.GreenBox, GUILayout.MinWidth(25), GUILayout.MaxWidth(25), GUILayout.ExpandHeight(true));
            
            EditorGUILayout.EndHorizontal();

            return selected;
        }
    }
}
