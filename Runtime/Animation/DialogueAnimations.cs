using System;
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

        public AnimationData GetAnimation(AnimationType type)
        {
            switch (type)
            {
                case AnimationType.Talk_1:
                    return talk1;
                case AnimationType.Talk_2:
                    return talk2;
                case AnimationType.Talk_Cocky:
                    return talkCocky;
                case AnimationType.Talk_Dismiss:
                    return talkDismiss;
                case AnimationType.Talk_Shrug:
                    return talkShrug;
                case AnimationType.Talk_Laugh:
                    return talkLaugh;
                case AnimationType.Talk_PonderQuestion:
                    return talkPonderQuestion;
                case AnimationType.Talk_Surprised:
                    return talkSurprised;
                case AnimationType.Talk_SighOfRelief:
                    return talkSighOfRelief;
                case AnimationType.Talk_HandsForward:
                    return talkHandsForward;
                case AnimationType.Agree_Nod:
                    return agreeNod;
                case AnimationType.Agree_Concede:
                    return agreeConcede;
                case AnimationType.Disagree:
                    return disagree;
                case AnimationType.Disagree_Annoyed:
                    return disagreeAnnoyed;
                case AnimationType.Celebrate_Clap:
                    return celebrateClap;
                case AnimationType.Celebrate_FistPump:
                    return celebrateFistPump;
                case AnimationType.Rage_Flex:
                    return rageFlex;
                case AnimationType.Rage_ShakeFist:
                    return rageShakeFist;
                case AnimationType.Rage_Yell1:
                    return rageYell1;
                case AnimationType.Rage_Yell2:
                    return rageYell2;
                case AnimationType.Disappointed_Curse:
                    return disappointedCurse;
                case AnimationType.Disappointed_HoldHead:
                    return disappointedHoldHead;
                case AnimationType.Disappointed_Mope:
                    return disappointedMope;
                case AnimationType.Point_Forward:
                    return pointForward;
                case AnimationType.Point_Backward:
                    return pointBackward;
                case AnimationType.RubShoulder:
                    return rubShoulder;
                case AnimationType.Wave:
                    return wave;
                default:
                    return null;
            }
        }
        
        public string GetAnimationName(AnimationType type)
        {
            AnimationData animation = GetAnimation(type);
            return animation != null ? animation.Name : string.Empty;
        }
        
        public float GetAnimationLength(AnimationType type)
        {
            AnimationData animation = GetAnimation(type);
            return animation?.Length ?? 0f;
        }
    }
}