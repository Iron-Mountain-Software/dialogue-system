using System;
using SpellBoundAR.SavedAssetsSystem;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [Serializable]
    public class SavedConversationData : SavedData
    {
        [SerializeField] private int playthroughs;
        
        public override void Save() { }

        public int Playthroughs
        {
            get => playthroughs;
            set
            {
                if (playthroughs == value) return;
                playthroughs = value;
                Save();
            }
        }

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
