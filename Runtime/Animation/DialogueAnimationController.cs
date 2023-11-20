using System.Collections.Generic;
using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using UnityEngine;

namespace IronMountain.DialogueSystem.Animation
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
            _animations.Add(AnimationType.Talk_Surprised, exclamation);
            _animations.Add(AnimationType.Disappointed_Mope, hunch);
            _animations.Add(AnimationType.Agree_Nod, nod);
            _animations.Add(AnimationType.Talk_PonderQuestion, question);
        }

        private void OnEnable() => ConversationPlayer.OnAnyDialogueLinePlayed += OnDialogueLinePlayed;
        private void OnDisable() => ConversationPlayer.OnAnyDialogueLinePlayed -= OnDialogueLinePlayed;

        private void OnDialogueLinePlayed(Conversation conversation, DialogueLine dialogueLine) 
        {
            if (!animator || !_speakerController || _speakerController.Speaker != dialogueLine.Speaker) return;
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