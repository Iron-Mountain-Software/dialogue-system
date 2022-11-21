using System.Collections.Generic;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Animation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IConversationEntity))]
    public class DialogueAnimationController : MonoBehaviour 
    {
        [SerializeField] private List<string> initialStates;
    
        [SerializeField] private string nod;
        [SerializeField] private string exclamation;
        [SerializeField] private string question;
        [SerializeField] private string hunch;

        [SerializeField] private Animator animator;

        [Header("Cache")]
        private IConversationEntity _entity;

        public string Nod => nod;
        public string Exclamation => exclamation;
        public string Question => question;
        public string Hunch => hunch;

        private void Start()
        {
            _entity = GetComponent<IConversationEntity>();
            ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        }

        private void OnDestroy()
        {
            ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;
        }

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine) 
        {
            if (!animator) return;
            if (_entity != conversation.Entity) return;
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            foreach (string initialState in initialStates)
            {
                if (!animatorStateInfo.IsName(initialState)) continue;
                switch (dialogueLine.Animation)
                {
                    case AnimationType.Nod:
                        PlayAnimation(Nod);
                        break;
                    case AnimationType.Exclamation:
                        PlayAnimation(Exclamation);
                        break;
                    case AnimationType.Question:
                        PlayAnimation(Question);
                        break;
                    case AnimationType.Hunch:
                        PlayAnimation(Hunch);
                        break;
                }
                break;
            }
        }

        public void PlayAnimation(string animationName)
        {
            if (!animator || string.IsNullOrWhiteSpace(animationName)) return;
            animator.Play(animationName);
        }
    }
}