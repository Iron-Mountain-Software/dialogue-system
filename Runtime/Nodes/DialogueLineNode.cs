using IronMountain.DialogueSystem.Animation;
using IronMountain.DialogueSystem.Speakers;
using IronMountain.DialogueSystem.UI;
using IronMountain.ResourceUtilities;
using UnityEngine;
using UnityEngine.Localization;

namespace IronMountain.DialogueSystem.Nodes
{
    [NodeWidth(300)]
    [NodeTint("#989898")]
    public class DialogueLineNode : DialogueNode
	{
		[Input] public Connection input;
		[Output] public Connection output;
		
		[SerializeField] private Speaker customSpeaker;
		[SerializeField] [TextArea(5,5)] private string simpleText;
		[SerializeField] private AudioClip audioClip;
		[SerializeField] private LocalizedString text;
		[SerializeField] private LocalizedAsset<AudioClip> localizedAudio;
		[SerializeField] protected SpeakerPortraitCollection.PortraitType portrait;
		[SerializeField] protected AnimationType animation;
		[SerializeField] protected ResourceSprite sprite;

		public Speaker CustomSpeaker
		{
			get => customSpeaker;
			set => customSpeaker = value;
		}

		public string SimpleText
		{
			get => simpleText;
			set => simpleText = value;
		}

		public string Text
		{
			get
			{
				if (Application.isPlaying)
				{
					return text.IsEmpty ? simpleText : text.GetLocalizedString();
				}
#if UNITY_EDITOR
				if (text.IsEmpty || string.IsNullOrEmpty(text.TableReference)) return simpleText;
				var collection = UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(text.TableReference);
				var entry = collection && collection.SharedData ? collection.SharedData.GetEntryFromReference(text.TableEntryReference) : null;
				return entry != null ? entry.Key : simpleText;
#else
				return string.Empty;
#endif
			}
		}

		public LocalizedString LocalizedText => text;

		public AudioClip AudioClip => !localizedAudio.IsEmpty && Application.isPlaying
				? localizedAudio.LoadAsset()
				: audioClip;

		protected virtual DialogueLine GetDialogueLine(ConversationPlayer conversationUI)
		{
			return new (
				customSpeaker ? customSpeaker : conversationUI.DefaultSpeaker,
				Text,
				AudioClip,
				portrait,
				animation,
				sprite ? sprite.Asset : null
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

		public override DialogueNode GetNextNode(ConversationPlayer conversationUI)
		{
			return GetOutputPort("output")?.Connection?.node as DialogueNode;
		}

		public override void OnNodeEnter(ConversationPlayer conversationUI)
		{
			base.OnNodeEnter(conversationUI);
			DialogueLine dialogueLine = GetDialogueLine(conversationUI);
			conversationUI.HandleDialogueLine(dialogueLine);
			DialogueNode nextHaltingNode = GetNextHaltingNode(conversationUI);
			if (nextHaltingNode is DialogueResponseBlockNode) conversationUI.CurrentNode = GetNextNode(conversationUI);
		}
		
#if UNITY_EDITOR

		private bool MissingAudioClip => !audioClip && localizedAudio.IsEmpty;
		private bool MissingText => string.IsNullOrWhiteSpace(simpleText) && (text.IsEmpty || string.IsNullOrEmpty(text.TableReference));

		public override void RefreshWarnings()
		{
			base.RefreshWarnings();
			if (MissingAudioClip) Warnings.Add("No audio.");
			if (MissingText) Warnings.Add("No text.");
		}
		
		public override void RefreshErrors()
		{
			base.RefreshErrors();
			if (GetInputPort("input").ConnectionCount == 0) Errors.Add("Bad input.");
			if (GetOutputPort("output").ConnectionCount != 1) Errors.Add("Bad output.");
		}

#endif
		
	}
}