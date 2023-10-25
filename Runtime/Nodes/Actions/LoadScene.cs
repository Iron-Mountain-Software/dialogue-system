using SpellBoundAR.DialogueSystem.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpellBoundAR.DialogueSystem.Nodes.Actions
{
    [NodeWidth(300)]
    [NodeTint("#FFCA3A")]
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