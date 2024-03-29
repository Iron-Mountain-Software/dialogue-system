using UnityEngine;

namespace IronMountain.DialogueSystem.Narration
{
    [ExecuteAlways]
    [RequireComponent(typeof(DialogueNarration))]
    public abstract class DialogueNarrationRequirement : MonoBehaviour
    {
        [Header("Cache")]
        protected DialogueNarration DialogueNarration;
        
        public abstract bool IsSatisfied();

        protected virtual void Awake()
        {
            DialogueNarration = GetComponent<DialogueNarration>();
        }
        
        protected virtual void OnEnable()
        {
            DialogueNarration.RefreshRequirements();
        }

        protected virtual void OnDisable()
        {
            DialogueNarration.RefreshRequirements();
        }
    }
}
