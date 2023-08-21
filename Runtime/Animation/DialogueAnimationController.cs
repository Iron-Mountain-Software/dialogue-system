using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Animation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpeakerController))]
    public class DialogueAnimationController : MonoBehaviour 
    {
        [SerializeField] private List<string> initialStates;
    
        [SerializeField] private string nod;
        [SerializeField] private string exclamation;
        [SerializeField] private string question;
        [SerializeField] private string hunch;

        [SerializeField] private Animator animator;

        private SpeakerController _speakerController;
        private readonly Dictionary<AnimationType, string> _animations = new ();

        private void Awake()
        {
            _speakerController = GetComponent<SpeakerController>();
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
                || !_speakerController
                || !conversation
                || conversation.Speaker == null
                || _speakerController.Speaker != conversation.Speaker) return;
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