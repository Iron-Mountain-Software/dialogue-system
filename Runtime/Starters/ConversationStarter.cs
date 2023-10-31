using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Starters
{
    public abstract class ConversationStarter : MonoBehaviour
    {
        public abstract ConversationPlayer StartConversation(ISpeaker speaker, Conversation conversation);
    }
}