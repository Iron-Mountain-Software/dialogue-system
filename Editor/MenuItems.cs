using IronMountain.DialogueSystem.Editor.Windows;
using UnityEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public static class MenuItems
    {
        [MenuItem("Iron Mountain/Dialogue System", priority = 2)]
        public static void OpenIndex()
        {
            ConversationIndex.Open();
        }
    }
}