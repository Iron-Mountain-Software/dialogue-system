using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    public class Textures : MonoBehaviour
    {
        private static Texture2D _validContainerTexture;
        public static Texture2D ValidContainerTexture {
            get
            {
                if (!_validContainerTexture) _validContainerTexture = InitializeTexture(new Color(0.2f, 0.2f, 0.2f));
                return _validContainerTexture;
            }
        }
        
        private static Texture2D _invalidContainerTexture;
        public static Texture2D InvalidContainerTexture {
            get
            {
                if (!_invalidContainerTexture) _invalidContainerTexture = InitializeTexture(new Color(0.41f, 0f, 0.04f));
                return _invalidContainerTexture;
            }
        }
        
        private static Texture2D _grayTexture;
        public static Texture2D GrayTexture {
            get
            {
                if (!_grayTexture) _grayTexture = InitializeTexture(new Color(0.29f, 0.29f, 0.29f));
                return _grayTexture;
            }
        }
        
        private static Texture2D _greenTexture;
        public static Texture2D GreenTexture {
            get
            {
                if (!_greenTexture) _greenTexture = InitializeTexture(new Color(0.09f, 0.78f, 0.11f));
                return _greenTexture;
            }
        }
        
        private static Texture2D _redTexture;
        public static Texture2D RedTexture {
            get
            {
                if (!_redTexture) _redTexture = InitializeTexture(new Color(0.76f, 0.11f, 0f));
                return _redTexture;
            }
        }
        
        private static Texture2D _yellowTexture;
        public static Texture2D YellowTexture {
            get
            {
                if (!_yellowTexture) _yellowTexture = InitializeTexture(new Color(1f, 0.61f, 0.08f));
                return _yellowTexture;
            }
        }
        
        private static Texture2D InitializeTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0,0, color);
            texture.Apply();
            return texture;
        }
    }
}