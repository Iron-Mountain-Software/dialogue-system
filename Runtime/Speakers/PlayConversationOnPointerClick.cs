using UnityEngine;
using UnityEngine.EventSystems;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    public class PlayConversationOnPointerClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpeakerController speakerController;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (speakerController) speakerController.StartConversation();
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (!speakerController) speakerController = GetComponentInParent<SpeakerController>();
            if (!speakerController) Debug.LogWarning("Warning: Missing a SpeakerController!", this);
        }
        
#endif

    }
}