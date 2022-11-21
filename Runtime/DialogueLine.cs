using SpellBoundAR.Characters;
using SpellBoundAR.VirtualCameraManagement;
using UnityEngine;

namespace ARISE.DialogueSystem
{
    public class DialogueLine
    {
        public string Text { get; }
        public AudioClip AudioClip { get; }
        public PortraitType Portrait { get; }
        public AnimationType Animation { get; }
        public Sprite Sprite { get; }
        public VirtualCameraReference StaticVirtualCameraReference { get; }

        public DialogueLine(
            string text,
            AudioClip audioClip,
            PortraitType portrait,
            AnimationType animation,
            Sprite sprite,
            VirtualCameraReference staticVirtualCameraReference)
        {
            Text = text;
            AudioClip = audioClip;
            Portrait = portrait;
            Animation = animation;
            Sprite = sprite;
            StaticVirtualCameraReference = staticVirtualCameraReference;
        }
    }
}