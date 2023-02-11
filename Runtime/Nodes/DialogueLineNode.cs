using SpellBoundAR.Characters;
using SpellBoundAR.DialogueSystem.Animation;
using SpellBoundAR.ResourceUtilities;
using SpellBoundAR.VirtualCameraManagement;
using UnityEngine;
using UnityEngine.Localization;

namespace SpellBoundAR.DialogueSystem.Nodes
{
    [NodeWidth(300)]
    [NodeTint("#989898")]
    public class DialogueLineNode : DialogueNode
	{
		[Input] public Connection input;
		[Output] public Connection output;
		
		[SerializeField] protected LocalizedString text;
		[SerializeField] protected AudioClip audioClip;
		[SerializeField] protected PortraitType portrait;
		[SerializeField] protected AnimationType animation;
		[SerializeField] protected ResourceSprite sprite;
		[SerializeField] protected VirtualCameraReference virtualCameraReference;

		public string Text
		{
			get
			{
				if (Application.isPlaying)
				{
					return text.IsEmpty ? string.Empty : text.GetLocalizedString();
				}
#if UNITY_EDITOR
				if (text.IsEmpty || string.IsNullOrEmpty(text.TableReference)) return string.Empty;
				var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
				var entry = collection && collection.SharedData ? collection.SharedData.GetEntryFromReference(text.TableEntryReference) : null;
				return entry != null ? entry.Key : string.Empty;
#else
				return string.Empty;
#endif
			}
		}

		public AudioClip AudioClip => audioClip;
		
		protected virtual DialogueLine GetDialogueLine()
		{
			return new (
				Text,
				audioClip,
				portrait,
				animation,
				sprite ? sprite.Asset : null,
				virtualCameraReference
			);
		}

		public override string Name
		{
			get
			{
#if UNITY_EDITOR
				string text = Text;
				if (string.IsNullOrWhiteSpace(text)) return "EMPTY LINE!";
				return text.Length > 44 ? text.Substring(0, 44) + "..." : text;
#else
				return "DIALOGUE LINE";
#endif
			}
		}

		public override DialogueNode GetNextNode(ConversationUI conversationUI)
		{
			return GetOutputPort("output")?.Connection?.node as DialogueNode;
		}

		public override void OnNodeEnter(ConversationUI conversationUI)
		{
			base.OnNodeEnter(conversationUI);
			DialogueLine dialogueLine = GetDialogueLine();
			conversationUI.PlayDialogueLine(dialogueLine);
			DialogueNode nextHaltingNode = GetNextHaltingNode(conversationUI);
			if (nextHaltingNode is DialogueResponseBlockNode) conversationUI.CurrentNode = GetNextNode(conversationUI);
		}
		
#if UNITY_EDITOR

		protected override bool ExtensionHasWarnings()
		{
			return !audioClip;
		}

		protected override bool ExtensionHasErrors()
		{
			return GetInputPort("input").ConnectionCount == 0
			       || GetOutputPort("output").ConnectionCount != 1
			       || text.IsEmpty
			       || string.IsNullOrEmpty(text.TableReference);
		}
		
#endif
		
	}
}