using System;
using SpellBoundAR.SavedAssetsSystem;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [Serializable]
    public class SavedConversationData : SavedData
    {
        [SerializeField] public int playthroughs;

        public SavedConversationData(Conversation conversation) : base(conversation)
        {
            playthroughs = 0;
        }

        public SavedConversationData()
        {
            playthroughs = 0;
        }
    }
}
