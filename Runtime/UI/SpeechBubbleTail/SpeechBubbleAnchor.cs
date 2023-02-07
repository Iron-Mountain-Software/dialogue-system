using SpellBoundAR.DialogueSystem.Entities;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    public class SpeechBubbleAnchor : MonoBehaviour
    {
        private IConversationEntity _conversationEntity;
        private void Awake() => _conversationEntity = GetComponentInParent<IConversationEntity>();
        private void OnEnable() => SpeechBubbleAnchorsManager.RegisterAnchor(_conversationEntity, transform);
        private void OnDisable() => SpeechBubbleAnchorsManager.UnregisterAnchor(_conversationEntity);
    }
}