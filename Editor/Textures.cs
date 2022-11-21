using UnityEngine;

namespace SpellBoundAR.DialogueSystem.Editor
{
    public class Textures : MonoBehaviour
    {
        private static Texture2D _containerTexture;
        public static Texture2D ContainerTexture {
            get
            {
                if (!_containerTexture) _containerTexture = InitializeTexture(new Color(0.2f, 0.2f, 0.2f));
                return _containerTexture;
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