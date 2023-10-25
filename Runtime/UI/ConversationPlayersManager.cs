using System;
using System.Collections.Generic;

namespace SpellBoundAR.DialogueSystem.UI
{
    public static class ConversationPlayersManager
    {
        public static event Action OnConversationPlayersChanged;

        public static readonly List<ConversationPlayer> ConversationPlayers = new ();

        public static void Register(ConversationPlayer conversationPlayer)
        {
            if (ConversationPlayers.Contains(conversationPlayer)) return;
            ConversationPlayers.Add(conversationPlayer);
            OnConversationPlayersChanged?.Invoke();
        }
        
        public static void Unregister(ConversationPlayer conversationPlayer)
        {
            if (!ConversationPlayers.Contains(conversationPlayer)) return;
            ConversationPlayers.Remove(conversationPlayer);
            OnConversationPlayersChanged?.Invoke();
        }
    }
}
