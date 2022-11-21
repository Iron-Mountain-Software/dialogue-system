using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    public class SpeechBubbleTailAnchor
    {
        private readonly IConversationEntity _conversationEntity;
        private readonly Transform _transform;

        public SpeechBubbleTailAnchor(IConversationEntity conversationEntity, Transform transform)
        {
            _conversationEntity = conversationEntity;
            _transform = transform;
        }

        public void Register()
        {
            SpeechBubbleTailManager.RegisterAnchor(_conversationEntity, _transform);
        }

        public void Unregister()
        {
            SpeechBubbleTailManager.UnregisterAnchor(_conversationEntity);
        }
    }
}