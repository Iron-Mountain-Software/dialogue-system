using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Starters
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