using System;
using System.Collections.Generic;
using System.Linq;
using IronMountain.DialogueSystem.Nodes;
using IronMountain.DialogueSystem.Nodes.Actions;
using IronMountain.DialogueSystem.Nodes.Conditions;
using IronMountain.DialogueSystem.Nodes.ResponseGenerators;
using IronMountain.DialogueSystem.Speakers;

namespace IronMountain.DialogueSystem.Editor
{
    public static class TypeIndex
    {
        public static readonly List<Type> SpeakerTypes;
        public static readonly List<Type> ConversationTypes;
        public static readonly List<Type> DialogueLineNodeTypes;
        public static readonly List<Type> DialogueResponseNodeTypes;
        public static readonly List<Type> DialogueActionNodeTypes;
        public static readonly List<Type> DialogueConditionNodeTypes;

        public static string[] SpeakerTypeNames => SpeakerTypes.Select(t => t.Name).ToArray();
        public static string[] ConversationTypeNames => ConversationTypes.Select(t => t.Name).ToArray();
        public static string[] DialogueLineNodeTypeNames => DialogueLineNodeTypes.Select(t => t.Name).ToArray();
        
        static TypeIndex()
        {
            SpeakerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(Speaker) || type.IsSubclassOf(typeof(Speaker))))
                .ToList();
            ConversationTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(Conversation) || type.IsSubclassOf(typeof(Conversation))))
                .ToList();
            DialogueLineNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(DialogueLineNode) || type.IsSubclassOf(typeof(DialogueLineNode))))
                .ToList();
            DialogueResponseNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(DialogueResponseNode) || type.IsSubclassOf(typeof(DialogueResponseNode))))
                .ToList();
            DialogueActionNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(DialogueAction) || type.IsSubclassOf(typeof(DialogueAction))))
                .ToList();
            DialogueConditionNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && (type == typeof(Condition) || type.IsSubclassOf(typeof(Condition))))
                .ToList();
        }
    }
}
