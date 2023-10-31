using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Starters
{
    public class ConversationStarterFromPrefab : ConversationStarter
    {
        [SerializeField] private ConversationPlayer prefab;
        
        public override ConversationPlayer StartConversation(ISpeaker speaker, Conversation conversation)
        {
            if (!enabled || !prefab || speaker == null || !conversation) return null;
            return Instantiate(prefab).Initialize(speaker, conversation);
        }
    }
}