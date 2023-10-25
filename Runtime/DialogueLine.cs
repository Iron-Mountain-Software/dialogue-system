using SpellBoundAR.DialogueSystem.Animation;
using SpellBoundAR.DialogueSystem.Speakers;
using SpellBoundAR.VirtualCameraManagement;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    public class DialogueLine
    {
        public ISpeaker Speaker { get; }
        public string Text { get; }
        public AudioClip AudioClip { get; }
        public SpeakerPortraitCollection.PortraitType Portrait { get; }
        public AnimationType Animation { get; }
        public Sprite Sprite { get; }
        public VirtualCameraReference VirtualCameraReference { get; }

        public DialogueLine(
            ISpeaker speaker,
            string text,
            AudioClip audioClip,
            SpeakerPortraitCollection.PortraitType portrait,
            AnimationType animation,
            Sprite sprite,
            VirtualCameraReference virtualCameraReference)
        {
            Speaker = speaker;
            Text = text;
            AudioClip = audioClip;
            Portrait = portrait;
            Animation = animation;
            Sprite = sprite;
            VirtualCameraReference = virtualCameraReference;
        }
    }
}