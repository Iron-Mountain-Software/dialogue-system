using SpellBoundAR.DialogueSystem.Entities;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Scripted Conversation")]
    public class ScriptedConversation : Conversation
    {
        [Header("Settings")]
        [SerializeField] private ScriptedConversationEntity scriptedConversationEntity;

        public override IConversationEntity Entity => scriptedConversationEntity;
    }
}
