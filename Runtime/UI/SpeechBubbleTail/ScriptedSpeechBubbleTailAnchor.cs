using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    public class ScriptedSpeechBubbleTailAnchor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScriptedConversationEntity conversationEntity;

        [Header("Cache")]
        private SpeechBubbleTailAnchor _anchor;
        
        private void Awake()
        {
            _anchor = new SpeechBubbleTailAnchor(conversationEntity, transform);
        }

        private void OnEnable() => _anchor?.Register();
        private void OnDisable() => _anchor?.Unregister();
    }
}