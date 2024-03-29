using IronMountain.DialogueSystem.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronMountain.DialogueSystem.Nodes.Actions
{
    [NodeWidth(300)]
    public class LoadScene : DialogueAction
    {
        [SerializeField] private string sceneName;

        protected override void HandleAction(ConversationPlayer conversationUI)
        {
            SceneManager.LoadScene(sceneName);
        }

        public override string Name => !string.IsNullOrWhiteSpace(sceneName) 
            ? "Load Scene: " + sceneName
            : "Load Scene: EMPTY!";
    }
}