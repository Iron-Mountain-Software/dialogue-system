using UnityEngine;

namespace IronMountain.DialogueSystem.Responses
{
    public interface IResponseStyle
    {
        public float Height { get; }
        public Color ButtonColorPrimary { get; }
        public Color ButtonColorSecondary { get; }
        public Color TextColor { get; }
    }
}
