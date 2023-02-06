using System;
using SpellBoundAR.SavedAssetsSystem;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [Serializable]
    public class ConversationSavedData : SavedData
    {
        [SerializeField] public int playthroughs;

        public ConversationSavedData(Conversation conversation) : base(conversation)
        {
            playthroughs = 0;
        }
    }
}
