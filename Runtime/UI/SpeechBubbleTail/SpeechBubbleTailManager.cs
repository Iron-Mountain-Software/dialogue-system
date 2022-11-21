using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    public static class SpeechBubbleTailManager
    {
        public static event Action OnClientsChanged;
        
        public static readonly Dictionary<IConversationEntity, Transform> Clients = new ();

        public static void RegisterAnchor(IConversationEntity client, Transform anchor)
        {
            if (client == null || !anchor) return;
            if (Clients.ContainsKey(client))
            {
                Clients[client] = anchor;
            }
            else Clients.Add(client, anchor);
            OnClientsChanged?.Invoke();
        }

        public static void UnregisterAnchor(IConversationEntity client)
        {
            if (client == null) return;
            if (!Clients.ContainsKey(client)) return;
            Clients.Remove(client);
            OnClientsChanged?.Invoke();
        }
    }
}
