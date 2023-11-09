using UnityEngine;
using UnityEngine.Localization;

namespace IronMountain.DialogueSystem.Nodes
{
    [System.Serializable]
    public class DialogueLineMainContent
    {
        [SerializeField] private LocalizedString text;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private LocalizedAsset<AudioClip> localizedAudio;
        public LocalizedString TextData => text;
        public string Text => text.IsEmpty ? string.Empty : text.GetLocalizedString();
        public AudioClip AudioClip
        {
            get
            {
                if (audioClip) return audioClip;
                return !localizedAudio.IsEmpty && Application.isPlaying ? localizedAudio.LoadAsset() : null;
            }
        }
    }
}