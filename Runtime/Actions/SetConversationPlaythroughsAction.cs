using System;
using IronMountain.ScriptableActions;
using UnityEngine;

namespace IronMountain.DialogueSystem.Actions
{
    public class SetConversationPlaythroughsAction : ScriptableAction
    {
        [SerializeField] private Conversation conversation;
        [SerializeField] private int playthroughs;

        public override void Invoke()
        {
            if (conversation) conversation.Playthroughs = playthroughs;
        }

        public override string ToString() => 
            "Set " + (conversation ? conversation.name : "NULL") + " playthroughs to " + playthroughs;

        public override bool HasErrors() => !conversation;

        private void OnValidate()
        {
            if (playthroughs < 0) playthroughs = 0;
        }
    }
}