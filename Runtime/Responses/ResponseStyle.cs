using UnityEngine;

namespace IronMountain.DialogueSystem.Responses
{
    public class ResponseStyle : IResponseStyle
    {
        public float Height { get; }
        public Color ButtonColorPrimary { get; }
        public Color ButtonColorSecondary { get; }
        public Color TextColor { get; }

        public ResponseStyle(
            float height,
            Color buttonColorPrimary,
            Color buttonColorSecondary,
            Color textColor)
        {
            Height = height;
            ButtonColorPrimary = buttonColorPrimary;
            ButtonColorSecondary = buttonColorSecondary;
            TextColor = textColor;
        }
    }
}
