using UnityEditor;

namespace IronMountain.DialogueSystem.Editor
{
    public static class MenuItems
    {
        [MenuItem("Iron Mountain/Conversations Window", priority = 2)]
        static void OpenEditorWindow()
        {
            ConversationsEditorWindow.Open();
        }
    }
}