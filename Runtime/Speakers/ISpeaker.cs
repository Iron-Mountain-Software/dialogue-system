using System;
using System.Collections.Generic;
using SpellBoundAR.AssetManagement;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    public interface ISpeaker : IIdentifiable
    {
        public event Action OnActiveConversationsChanged;

        public Conversation DefaultConversation { get; }
        public List<Conversation> Conversations { get; }
        public SpeakerPortraitCollection Portraits { get; }
    }
}