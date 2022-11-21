using SpellBoundAR.AssetManagement;
using UnityEngine;

namespace SpellBoundAR.DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Gameplay/Dialogue/Database")]
    public class Database : SingletonDatabase<Database>
    {
        [SerializeField] private DatabaseTable<Conversation> conversations = new ();
        public DatabaseTable<Conversation> Conversations => conversations;
    }
}