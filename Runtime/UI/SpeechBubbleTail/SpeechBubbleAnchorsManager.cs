using System;
using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    public static class SpeechBubbleAnchorsManager
    {
        public static event Action OnAnchorsChanged;
        
        public static readonly List<SpeechBubbleAnchor> Anchors = new ();

        public static void RegisterAnchor(SpeechBubbleAnchor anchor)
        {
            if (!anchor || Anchors.Contains(anchor)) return;
            Anchors.Add(anchor);
            OnAnchorsChanged?.Invoke();
        }

        public static void UnregisterAnchor(SpeechBubbleAnchor anchor)
        {
            if (!anchor || !Anchors.Contains(anchor)) return;
            Anchors.Remove(anchor);
            OnAnchorsChanged?.Invoke();
        }
        
        public static Transform GetAnchor(ISpeaker speaker)
        {
            if (speaker == null) return null;
            return Anchors.Find(test =>
                test
                && test.SpeakerController
                && test.SpeakerController.Speaker == speaker).transform;
        }
    }
}
