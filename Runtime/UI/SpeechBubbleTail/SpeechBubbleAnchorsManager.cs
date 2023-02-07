using System;
using System.Collections.Generic;
using SpellBoundAR.DialogueSystem.Entities;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem.UI.SpeechBubbleTail
{
    public static class SpeechBubbleAnchorsManager
    {
        public static event Action OnAnchorsChanged;
        
        public static readonly Dictionary<string, Transform> Anchors = new ();

        public static void RegisterAnchor(IConversationEntity client, Transform anchor)
        {
            if (client == null || !anchor) return;
            if (Anchors.ContainsKey(client.ID))
            {
                Anchors[client.ID] = anchor;
            }
            else Anchors.Add(client.ID, anchor);
            OnAnchorsChanged?.Invoke();
        }

        public static Transform GetAnchor(IConversationEntity client)
        {
            if (client == null) return null;
            return Anchors.ContainsKey(client.ID)
                ? Anchors[client.ID]
                : null;
        }

        public static void UnregisterAnchor(IConversationEntity client)
        {
            if (client == null) return;
            if (!Anchors.ContainsKey(client.ID)) return;
            Anchors.Remove(client.ID);
            OnAnchorsChanged?.Invoke();
        }
    }
}
