using System;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Speakers
{
    [Serializable]
    public class SpeakerPortraitCollection
    {
        public enum PortraitType
        {
            Happy,
            Neutral,
            Sad
        }

        [SerializeField] private Sprite happyPortrait;
        [SerializeField] private Sprite neutralPortrait;
        [SerializeField] private Sprite sadPortrait;

        public Sprite GetPortrait(PortraitType type)
        {
            return type switch
            {
                PortraitType.Happy => happyPortrait,
                PortraitType.Neutral => neutralPortrait,
                PortraitType.Sad => sadPortrait,
                _ => null
            };
        }
    }
}