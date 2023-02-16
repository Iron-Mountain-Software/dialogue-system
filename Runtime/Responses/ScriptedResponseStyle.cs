using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Responses
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Response Style")]
    public class ScriptedResponseStyle : ScriptableObject, IResponseStyle
    {
        [SerializeField] [Range(0, 1)] private float height;
        [SerializeField] private Color buttonColorPrimary;
        [SerializeField] private Color buttonColorSecondary;
        [SerializeField] private Color textColor;

        public float Height => height;
        public Color ButtonColorPrimary => buttonColorPrimary;
        public Color ButtonColorSecondary => buttonColorSecondary;
        public Color TextColor => textColor;
    }
}