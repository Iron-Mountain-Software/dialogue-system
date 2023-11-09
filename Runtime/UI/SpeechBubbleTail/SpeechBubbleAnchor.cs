using IronMountain.DialogueSystem.Speakers;
using UnityEngine;

namespace IronMountain.DialogueSystem.UI.SpeechBubbleTail
{
    public class SpeechBubbleAnchor : MonoBehaviour
    {
        [Header("Cache")]
        private SpeakerController _speakerController;

        public SpeakerController SpeakerController => _speakerController;
        
        private void Awake() => _speakerController = GetComponentInParent<SpeakerController>();
        private void OnEnable() => SpeechBubbleAnchorsManager.RegisterAnchor(this);
        private void OnDisable() => SpeechBubbleAnchorsManager.UnregisterAnchor(this);
    }
}