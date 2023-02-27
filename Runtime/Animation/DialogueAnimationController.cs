using System;
using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Entities;
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

        private IConversationEntity _entity;
        private readonly Dictionary<AnimationType, string> _animations = new ();

        private void Awake()
        {
            _entity = GetComponent<IConversationEntity>();
            _animations.Add(AnimationType.Exclamation, exclamation);
            _animations.Add(AnimationType.Hunch, hunch);
            _animations.Add(AnimationType.Nod, nod);
            _animations.Add(AnimationType.Question, question);
        }

        private void OnEnable() => ConversationUI.OnDialogueLinePlayed += OnDialogueLinePlayed;
        private void OnDisable() => ConversationUI.OnDialogueLinePlayed -= OnDialogueLinePlayed;

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine) 
        {
            if (!animator
                || _entity == null
                || !conversation
                || conversation.Entity == null
                || _entity.ID != conversation.Entity.ID) return;
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            foreach (string initialState in initialStates)
            {
                if (!animatorStateInfo.IsName(initialState)) continue;
                PlayAnimation(dialogueLine.Animation);
                break;
            }
        }

        public void PlayAnimation(AnimationType animationType)
        {
            if (!animator) return;
            if (!_animations.ContainsKey(animationType)) return;
            if (string.IsNullOrWhiteSpace(_animations[animationType])) return;
            animator.Play(_animations[animationType]);
        }
    }
}