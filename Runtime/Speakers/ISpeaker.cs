using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    public interface ISpeaker
    {
        public event Action OnActiveConversationsChanged;

        public string ID { get; }
        public string SpeakerName { get; }
        public Color Color { get; }
        public Conversation DefaultConversation { get; }
        public List<Conversation> Conversations { get; }
        public SpeakerPortraitCollection Portraits { get; }
        public SpeakerPortraitCollection FullBodyPortraits { get; }
    }
}