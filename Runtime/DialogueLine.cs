using IronMountain.DialogueSystem.Animation;
using IronMountain.DialogueSystem.Speakers;
using UnityEngine;

namespace IronMountain.DialogueSystem
{
    public class DialogueLine
    {
        public ISpeaker Speaker { get; }
        public string Text { get; }
        public AudioClip AudioClip { get; }
        public SpeakerPortraitCollection.PortraitType Portrait { get; }
        public AnimationType Animation { get; }
        public Sprite Sprite { get; }

        public DialogueLine(
            ISpeaker speaker,
            string text,
            AudioClip audioClip,
            SpeakerPortraitCollection.PortraitType portrait,
            AnimationType animation,
            Sprite sprite)
        {
            Speaker = speaker;
            Text = text;
            AudioClip = audioClip;
            Portrait = portrait;
            Animation = animation;
            Sprite = sprite;
        }
    }
}