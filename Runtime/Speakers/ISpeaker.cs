using System;
using System.Collections.Generic;
using UnityEngine;

namespace IronMountain.DialogueSystem.Speakers
{
    public interface ISpeaker
    {
        public event Action OnActiveConversationsChanged;

        public string ID { get; }
        public string SpeakerName { get; }
        public List<string> Aliases { get; }
        public Color Color { get; }
        public Conversation DefaultConversation { get; }
        public List<Conversation> Conversations { get; }
        public SpeakerPortraitCollection Portraits { get; }
        public SpeakerPortraitCollection FullBodyPortraits { get; }

        public bool UsesNameOrAlias(string name)
        {
            name = name.ToLower();
            return string.Equals(SpeakerName.ToLower(), name) 
                   || !string.IsNullOrWhiteSpace(Aliases?.Find(test => 
                       !string.IsNullOrWhiteSpace(test) 
                       && string.Equals(test.ToLower(), name)));
        }
    }
}