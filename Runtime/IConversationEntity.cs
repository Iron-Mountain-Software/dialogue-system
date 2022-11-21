using System.Collections.Generic;
using SpellBoundAR.AssetManagement;
using SpellBoundAR.Characters;
using UnityEngine;

namespace ARISE.DialogueSystem
{
    public interface IConversationEntity : IIdentifiable, IDepictable
    {
        public Conversation DefaultConversation { get; }
        public List<Conversation> Conversations { get; }

        public List<Conversation> GetActiveDialogue();
        public Conversation GetNextPrioritizedConversation();
        public Conversation GetNextConversation();
        public Sprite GetPortrait(PortraitType type);
    }
}
