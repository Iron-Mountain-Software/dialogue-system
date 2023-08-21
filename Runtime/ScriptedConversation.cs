using SpellBoundAR.DialogueSystem.Speakers;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue/Scripted Conversation")]
    public class ScriptedConversation : Conversation
    {
        [Header("Settings")]
        [SerializeField] private Speaker speaker;
    }
}
