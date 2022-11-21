using UnityEngine;
using UnityEngine.Localization;

namespace SpellBoundAR.DialogueSystem.Nodes
{
    [System.Serializable]
    public class DialogueLineMainContent
    {
        [SerializeField] private LocalizedString text;
        [SerializeField] private AudioClip audioClip;
        public LocalizedString TextData => text;
        public string Text => text.IsEmpty ? string.Empty : text.GetLocalizedString();
        public AudioClip AudioClip => audioClip;
    }
}