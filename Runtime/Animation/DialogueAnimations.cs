using System;
using System.Collections.Generic;
using UnityEngine;

namespace IronMountain.DialogueSystem.Animation
{
    [Serializable]
    public class DialogueAnimations
    {
        [Serializable]
        public class AnimationData
        {
            public AnimationClip clip;
            public string Name => clip ? clip.name : string.Empty;
            public float Length => clip ? clip.length : 0f;
        }

        private Dictionary<AnimationType, AnimationData> _standardAnimations;
        
        [SerializeField] private AnimationData talk1;
        [SerializeField] private AnimationData talk2;
        [SerializeField] private AnimationData talkCocky;
        [SerializeField] private AnimationData talkDismiss;
        [SerializeField] private AnimationData talkShrug;
        [SerializeField] private AnimationData talkLaugh;
        [SerializeField] private AnimationData talkPonderQuestion;
        [SerializeField] private AnimationData talkSurprised;
        [SerializeField] private AnimationData talkSighOfRelief;
        [SerializeField] private AnimationData talkHandsForward;
        [SerializeField] private AnimationData agreeNod;
        [SerializeField] private AnimationData agreeConcede;
        [SerializeField] private AnimationData disagree;
        [SerializeField] private AnimationData disagreeAnnoyed;
        [SerializeField] private AnimationData celebrateClap;
        [SerializeField] private AnimationData celebrateFistPump;
        [SerializeField] private AnimationData rageFlex;
        [SerializeField] private AnimationData rageShakeFist;
        [SerializeField] private AnimationData rageYell1;
        [SerializeField] private AnimationData rageYell2;
        [SerializeField] private AnimationData disappointedCurse;
        [SerializeField] private AnimationData disappointedHoldHead;
        [SerializeField] private AnimationData disappointedMope;
        [SerializeField] private AnimationData pointForward;
        [SerializeField] private AnimationData pointBackward;
        [SerializeField] private AnimationData rubShoulder;
        [SerializeField] private AnimationData wave;

        public string GetAnimationName(AnimationType type)
        {
            return _standardAnimations.ContainsKey(type) && _standardAnimations[type] != null
                ? _standardAnimations[type].Name 
                : string.Empty;
        }
        
        public float GetAnimationLength(AnimationType type)
        {
            return _standardAnimations.ContainsKey(type) && _standardAnimations[type] != null
                ? _standardAnimations[type].Length
                : 0f;
        }

        DialogueAnimations()
        {
            _standardAnimations = new Dictionary<AnimationType, AnimationData>()
            {
                { AnimationType.Talk_1, talk1 },
                { AnimationType.Talk_2, talk2 },
                { AnimationType.Talk_Cocky, talkCocky },
                { AnimationType.Talk_Dismiss, talkDismiss },
                { AnimationType.Talk_Shrug, talkShrug },
                { AnimationType.Talk_Laugh, talkLaugh },
                { AnimationType.Talk_PonderQuestion, talkPonderQuestion },
                { AnimationType.Talk_Surprised, talkSurprised },
                { AnimationType.Talk_SighOfRelief, talkSighOfRelief },
                { AnimationType.Talk_HandsForward, talkHandsForward },
                { AnimationType.Agree_Nod, agreeNod },
                { AnimationType.Agree_Concede, agreeConcede },
                { AnimationType.Disagree, disagree },
                { AnimationType.Disagree_Annoyed, disagreeAnnoyed },
                { AnimationType.Celebrate_Clap, celebrateClap },
                { AnimationType.Celebrate_FistPump, celebrateFistPump },
                { AnimationType.Rage_Flex, rageFlex },
                { AnimationType.Rage_ShakeFist, rageShakeFist },
                { AnimationType.Rage_Yell1, rageYell1 },
                { AnimationType.Rage_Yell2, rageYell2 },
                { AnimationType.Disappointed_Curse, disappointedCurse },
                { AnimationType.Disappointed_HoldHead, disappointedHoldHead },
                { AnimationType.Disappointed_Mope, disappointedMope },
                { AnimationType.Point_Forward, pointForward },
                { AnimationType.Point_Backward, pointBackward },
                { AnimationType.RubShoulder, rubShoulder },
                { AnimationType.Wave, wave },
            };
        }
    }
}