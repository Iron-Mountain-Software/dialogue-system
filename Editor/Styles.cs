using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    public static class Styles
    {
        public static readonly GUIStyle GrayBox = new ()
        {
            margin = new RectOffset(2, 2, 1, 1),
            alignment = TextAnchor.MiddleCenter,
            normal = {textColor = Color.white, background = Textures.GrayTexture}
        };
        
        public static readonly GUIStyle RedBox = new ()
        {
            margin = new RectOffset(2, 2, 1, 1),
            alignment = TextAnchor.MiddleCenter,
            normal = {textColor = Color.white, background = Textures.RedTexture}
        };
        
        public static readonly GUIStyle YellowBox = new ()
        {
            margin = new RectOffset(2, 2, 1, 1),
            alignment = TextAnchor.MiddleCenter,
            normal = {textColor = Color.white, background = Textures.YellowTexture}
        };
        
        public static readonly GUIStyle GreenBox = new ()
        {
            margin = new RectOffset(2, 2, 1, 1),
            alignment = TextAnchor.MiddleCenter,
            normal = {textColor = Color.white, background = Textures.GreenTexture}
        };
        
        public static readonly GUIStyle Container = new ()
        {
            padding = new RectOffset(7,7,7,7),
            normal = { background = Textures.ContainerTexture }
        };

        public static readonly GUIStyle Header = new ()
        {
            alignment = TextAnchor.LowerLeft,
            fontSize = 15,
            padding = new RectOffset(2,2,2,2),
            fontStyle = FontStyle.Bold,
            normal = {textColor = new Color(0.45f, 0.45f, 0.45f)}
        };
    }
}