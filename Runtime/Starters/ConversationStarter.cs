using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Starters
{
    public abstract class ConversationStarter : MonoBehaviour
    {
        public abstract ConversationPlayer StartConversation(ISpeaker speaker, Conversation conversation);
    }
}