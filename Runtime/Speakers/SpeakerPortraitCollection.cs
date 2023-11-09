using System;
using IronMountain.ResourceUtilities;
using UnityEngine;

namespace IronMountain.DialogueSystem.Speakers
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
        [SerializeField] private ResourceSprite happyPortraitResource;
        [SerializeField] private Sprite neutralPortrait;
        [SerializeField] private ResourceSprite neutralPortraitResource;
        [SerializeField] private Sprite sadPortrait;
        [SerializeField] private ResourceSprite sadPortraitResource;

        public Sprite GetPortrait(PortraitType type)
        {
            return type switch
            {
                PortraitType.Happy => happyPortrait ? happyPortrait : happyPortraitResource ? happyPortraitResource.Asset : null,
                PortraitType.Neutral => neutralPortrait ? neutralPortrait : neutralPortraitResource ? neutralPortraitResource.Asset : null,
                PortraitType.Sad => sadPortrait ? sadPortrait : sadPortraitResource ? sadPortraitResource.Asset : null,
                _ => null
            };
        }
    }
}