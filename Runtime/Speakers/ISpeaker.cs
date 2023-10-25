using System;
using System.Collections.Generic;
using SpellBoundAR.AssetManagement;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    public interface ISpeaker : IIdentifiable
    {
        public event Action OnActiveConversationsChanged;

        public string SpeakerName { get; }
        public Color Color { get; }
        public Conversation DefaultConversation { get; }
        public List<Conversation> Conversations { get; }
        public SpeakerPortraitCollection Portraits { get; }
        public SpeakerPortraitCollection FullBodyPortraits { get; }
    }
}