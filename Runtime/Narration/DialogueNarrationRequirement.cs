using UnityEngine;

namespace IronMountain.DialogueSystem.Narration
{
    [ExecuteAlways]
    [RequireComponent(typeof(DialogueNarrator))]
    public abstract class DialogueNarrationRequirement : MonoBehaviour
    {
        [Header("Cache")]
        protected DialogueNarrator DialogueNarrator;
        
        public abstract bool IsSatisfied();

        protected virtual void Awake()
        {
            DialogueNarrator = GetComponent<DialogueNarrator>();
        }
        
        protected virtual void OnEnable()
        {
            DialogueNarrator.RefreshRequirements();
        }

        protected virtual void OnDisable()
        {
            DialogueNarrator.RefreshRequirements();
        }
    }
}
