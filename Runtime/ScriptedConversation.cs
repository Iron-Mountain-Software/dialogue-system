using UnityEngine;

namespace ARISE.DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Scripted Conversation")]
    public class ScriptedConversation : Conversation
    {
        [Header("Settings")]
        [SerializeField] private ScriptedConversationEntity scriptedConversationEntity;

        public override IConversationEntity Entity => scriptedConversationEntity;
    }
}
