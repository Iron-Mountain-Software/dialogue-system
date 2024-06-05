# Dialogue System
*Version: 3.3.2*
## Description: 
A system for writing and playing branching dialogue.
## Dependencies: 
* com.unity.localization (1.3.2)
* com.unity.textmeshpro (3.0.6)
* com.github.siccity.xnode (1.8.0)
* com.iron-mountain.save-system (1.0.4)
* com.iron-mountain.conditions (1.5.8)
* com.iron-mountain.resource-utilities (1.1.3)
* com.iron-mountain.scriptable-actions (1.0.6)
## Package Mirrors: 
[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODg3LnBuZw==/original/npRUfq.png'>](https://github.com/Iron-Mountain-Software/dialogue-system)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODkyLnBuZw==/original/Fq0ORM.png'>](https://www.npmjs.com/package/com.iron-mountain.dialogue-system)[<img src='https://img.itch.zone/aW1nLzEzNzQ2ODk4LnBuZw==/original/Rv4m96.png'>](https://iron-mountain.itch.io/dialogue-system)
---
## Key Scripts & Components: 
1. public class **Conversation** : NodeGraph
   * Actions: 
      * public event Action ***OnIsActiveChanged*** 
      * public event Action ***OnPlaythroughsChanged*** 
   * Properties: 
      * public String ***ID***  { get; set; }
      * public String ***Name***  { get; }
      * public Boolean ***PrioritizeOverDefault***  { get; }
      * public Int32 ***Priority***  { get; }
      * public String ***DefaultInvokingLine***  { get; set; }
      * public String ***InvokingLine***  { get; }
      * public Sprite ***InvokingIcon***  { get; }
      * public Boolean ***AlertInConversationMenu***  { get; }
      * public ConversationPreviewType ***PreviewType***  { get; }
      * public String ***PreviewText***  { get; }
      * public Condition ***Condition***  { get; set; }
      * public BehaviorWhenQueued ***BehaviorWhenEnqueued***  { get; }
      * public Boolean ***Looping***  { get; }
      * public Boolean ***IsActive***  { get; }
      * public Int32 ***Playthroughs***  { get; set; }
      * public Boolean ***PreviewHasErrors***  { get; }
   * Methods: 
      * public void ***RefreshActiveState***()
      * public virtual void ***OnConversationStarted***()
      * public void ***OnValidate***()
      * public Boolean ***HasWarnings***()
      * public Boolean ***HasErrors***()
      * public virtual void ***Reset***()
      * public void ***GenerateNewID***()
      * public virtual void ***RefreshWarnings***()
      * public virtual void ***RefreshErrors***()
1. public class **ConversationPlayer** : MonoBehaviour
   * Actions: 
      * public event Action ***OnEnabledChanged*** 
      * public event Action ***OnDefaultSpeakerChanged*** 
      * public event Action ***OnConversationChanged*** 
      * public event Action ***OnDialogueNodeChanged*** 
      * public event Action ***OnDialogueLinePlayed*** 
      * public event Action ***OnIsMutedChanged*** 
      * public event Action ***OnClosed*** 
   * Properties: 
      * public Boolean ***AutoAdvance***  { get; }
      * public float ***AutoAdvanceSeconds***  { get; }
      * public float ***TotalSecondsToRespond***  { get; set; }
      * public float ***SecondsRemainingToRespond***  { get; set; }
      * public Int32 ***FrameOfLastProgression***  { get; }
      * public float ***TimeOfLastProgression***  { get; }
      * public float ***Timer***  { get; set; }
      * public ISpeaker ***DefaultSpeaker***  { get; }
      * public Conversation ***Conversation***  { get; }
      * public DialogueNode ***CurrentNode***  { get; set; }
      * public DialogueLine ***CurrentDialogueLine***  { get; set; }
      * public Boolean ***IsMuted***  { get; set; }
   * Methods: 
      * public ConversationPlayer ***Initialize***(ISpeaker speaker, Conversation conversation)
      * public void ***Close***()
      * public void ***SpawnResponseBlock***(DialogueResponseBlockNode dialogueResponseBlockNode)
      * public void ***CloseResponseBlock***(DialogueResponseBlockNode dialogueResponseBlockNode)
      * public void ***PlayNextNode***()
      * public void ***CompleteConversation***()
1. public static class **ConversationPlayersManager**
1. public enum **ConversationPreviewType** : Enum
1. public class **ConversationsManager** : MonoBehaviour
1. public class **DialogueInputManager** : MonoBehaviour
1. public class **DialogueLine**
   * Properties: 
      * public ISpeaker ***Speaker***  { get; }
      * public String ***Text***  { get; }
      * public AudioClip ***AudioClip***  { get; }
      * public PortraitType ***Portrait***  { get; }
      * public AnimationType ***Animation***  { get; }
      * public Sprite ***Sprite***  { get; }
### Actions
1. public class **SetConversationPlaythroughsAction** : ScriptableAction
   * Methods: 
      * public override void ***Invoke***()
      * public override String ***ToString***()
      * public override Boolean ***HasErrors***()
### Animation
1. public enum **AnimationType** : Enum
1. public class **DialogueAnimationController** : MonoBehaviour
   * Methods: 
      * public void ***PlayAnimation***(AnimationType animationType)
1. public class **DialogueAnimations**
   * Methods: 
      * public AnimationData ***GetAnimation***(AnimationType type)
      * public String ***GetAnimationName***(AnimationType type)
      * public float ***GetAnimationLength***(AnimationType type)
### Conditions
1. public class **ConditionConversationPlaythroughs** : Condition
   * Properties: 
      * public Sprite ***Depiction***  { get; }
   * Methods: 
      * public override Boolean ***Evaluate***()
      * public override Boolean ***HasErrors***()
      * public override String ***ToString***()
### Dialogue Bubbles
1. public class **DialogueBubble** : MonoBehaviour
1. public class **DialogueBubbleAnimator** : MonoBehaviour
   * Methods: 
      * public void ***ScaleUp***()
      * public void ***ScaleUp***(float duration)
      * public void ***ScaleUpImmediate***()
      * public void ***ScaleDown***()
      * public void ***ScaleDown***(float duration)
      * public void ***ScaleDownImmediate***()
### Narration
1. public static class **DialogueNarrationManager**
1. public abstract class **DialogueNarrationRequirement** : MonoBehaviour
   * Methods: 
      * public abstract Boolean ***IsSatisfied***()
1. public class **DialogueNarrator** : MonoBehaviour
   * Actions: 
      * public event Action ***OnIsPlayingChanged*** 
   * Properties: 
      * public ISpeaker ***Speaker***  { get; }
      * public AudioSource ***AudioSource***  { get; }
      * public ConversationPlayer ***CurrentConversationPlayer***  { get; }
      * public Boolean ***IsPlaying***  { get; set; }
   * Methods: 
      * public void ***RefreshRequirements***()
### Nodes
1. public struct **Connection**
1. public class **DialogueBeginningNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshErrors***()
1. public class **DialogueEndingNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshErrors***()
1. public class **DialogueLineMainContent**
   * Properties: 
      * public LocalizedString ***TextData***  { get; }
      * public String ***Text***  { get; }
      * public AudioClip ***AudioClip***  { get; }
1. public class **DialogueLineNode** : DialogueNode
   * Properties: 
      * public Speaker ***CustomSpeaker***  { get; set; }
      * public String ***SimpleText***  { get; set; }
      * public String ***Text***  { get; }
      * public LocalizedString ***LocalizedText***  { get; }
      * public AudioClip ***AudioClip***  { get; }
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshWarnings***()
      * public override void ***RefreshErrors***()
1. public class **DialogueLineWithAlternatesNode** : DialogueLineNode
   * Properties: 
      * public List<DialogueLineMainContent> ***AlternateContent***  { get; }
   * Methods: 
      * public override void ***RefreshWarnings***()
1. public abstract class **DialogueNode** : Node
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public abstract DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public abstract void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public abstract void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public abstract void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public DialogueNode ***GetNextHaltingNode***(ConversationPlayer conversationUI)
      * public override void ***OnCreateConnection***(NodePort from, NodePort to)
      * public override void ***OnRemoveConnection***(NodePort port)
      * public Boolean ***HasWarnings***()
      * public Boolean ***HasErrors***()
      * public virtual void ***RefreshWarnings***()
      * public virtual void ***RefreshErrors***()
1. public class **DialoguePassNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshErrors***()
1. public class **DialogueRandomizerNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshErrors***()
1. public class **DialogueResponseBlockNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
      * public Boolean ***IsTimed***  { get; }
      * public float ***Seconds***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public DialogueNode ***GetDefaultResponseNode***()
      * public List`1 ***GetResponses***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***OnCreateConnection***(NodePort from, NodePort to)
      * public override void ***RefreshErrors***()
### Nodes. Actions
1. public abstract class **DialogueAction** : DialogueNode
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshErrors***()
1. public class **DialoguePlayPrioritized** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
1. public class **LoadScene** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
1. public class **ScriptableActionNode** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
1. public class **UnityEvent** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
### Nodes. Conditions
1. public abstract class **Condition** : DialogueNode
   * Methods: 
      * public override void ***RefreshErrors***()
1. public abstract class **PassFailCondition** : Condition
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshErrors***()
1. public class **PassFailConditionDialogueQueued** : PassFailCondition
   * Properties: 
      * public String ***Name***  { get; }
1. public class **PassFailConditionFromReference** : PassFailCondition
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override void ***RefreshErrors***()
### Nodes. Response Generators
1. public class **BasicDialogueResponseNode** : DialogueResponseNode
   * Properties: 
      * public String ***Name***  { get; }
      * public String ***Text***  { get; }
      * public LocalizedString ***LocalizedText***  { get; }
   * Methods: 
      * public override List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
      * public override void ***RefreshWarnings***()
1. public abstract class **DialogueResponseNode** : DialogueNode
   * Properties: 
      * public ScriptedResponseStyle ***ScriptedResponseStyle***  { get; }
   * Methods: 
      * public abstract List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeUpdate***(ConversationPlayer conversationPlayer)
      * public override void ***OnNodeExit***(ConversationPlayer conversationPlayer)
      * public override void ***OnCreateConnection***(NodePort from, NodePort to)
      * public override void ***RefreshErrors***()
1. public class **DialogueResponseNodeActiveDialogue** : DialogueResponseNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
### Responses
1. public abstract class **BasicResponse**
   * Properties: 
      * public String ***Text***  { get; }
      * public Sprite ***Icon***  { get; }
      * public Int32 ***Row***  { get; }
      * public Int32 ***Column***  { get; }
      * public IResponseStyle ***Style***  { get; }
   * Methods: 
      * public abstract void ***Execute***()
1. public class **BranchConversationResponse** : BasicResponse
   * Methods: 
      * public override void ***Execute***()
1. public interface **IResponseStyle**
   * Properties: 
      * public float ***Height***  { get; }
      * public Color ***ButtonColorPrimary***  { get; }
      * public Color ***ButtonColorSecondary***  { get; }
      * public Color ***TextColor***  { get; }
1. public class **ResponseStyle**
   * Properties: 
      * public float ***Height***  { get; }
      * public Color ***ButtonColorPrimary***  { get; }
      * public Color ***ButtonColorSecondary***  { get; }
      * public Color ***TextColor***  { get; }
1. public class **ScriptedResponseStyle** : ScriptableObject
   * Properties: 
      * public float ***Height***  { get; }
      * public Color ***ButtonColorPrimary***  { get; }
      * public Color ***ButtonColorSecondary***  { get; }
      * public Color ***TextColor***  { get; }
1. public class **SwapConversationResponse** : BasicResponse
   * Methods: 
      * public override void ***Execute***()
### Selection
1. public abstract class **ConversationSelector** : MonoBehaviour
   * Actions: 
      * public event Action ***OnNextConversationChanged*** 
   * Properties: 
      * public ISpeaker ***Speaker***  { get; set; }
      * public Conversation ***NextConversation***  { get; }
   * Methods: 
      * public abstract void ***RefreshNextConversation***()
1. public class **DefaultConversationSelector** : ConversationSelector
   * Methods: 
      * public override void ***RefreshNextConversation***()
1. public class **PrioritizedConversationSelector** : ConversationSelector
   * Methods: 
      * public override void ***RefreshNextConversation***()
### Speakers
1. public interface **ISpeaker**
   * Actions: 
      * public event Action ***OnActiveConversationsChanged*** 
   * Properties: 
      * public String ***ID***  { get; }
      * public String ***SpeakerName***  { get; }
      * public List<String> ***Aliases***  { get; }
      * public Color ***Color***  { get; }
      * public Conversation ***DefaultConversation***  { get; }
      * public List<Conversation> ***Conversations***  { get; }
      * public SpeakerPortraitCollection ***Portraits***  { get; }
      * public SpeakerPortraitCollection ***FullBodyPortraits***  { get; }
   * Methods: 
      * public virtual Boolean ***UsesNameOrAlias***(String name)
1. public class **PlayConversationOnPointerClick** : MonoBehaviour
   * Methods: 
      * public virtual void ***OnPointerClick***(PointerEventData eventData)
1. public class **Speaker** : ScriptableObject
   * Actions: 
      * public event Action ***OnActiveConversationsChanged*** 
   * Properties: 
      * public String ***ID***  { get; }
      * public String ***SpeakerName***  { get; }
      * public List<String> ***Aliases***  { get; }
      * public Color ***Color***  { get; }
      * public Conversation ***DefaultConversation***  { get; }
      * public List<Conversation> ***Conversations***  { get; }
      * public SpeakerPortraitCollection ***Portraits***  { get; }
      * public SpeakerPortraitCollection ***FullBodyPortraits***  { get; }
   * Methods: 
      * public virtual void ***Reset***()
1. public class **SpeakerController** : MonoBehaviour
   * Actions: 
      * public event Action ***OnSpeakerChanged*** 
      * public event Action ***OnEnabled*** 
      * public event Action ***OnDisabled*** 
   * Properties: 
      * public ISpeaker ***Speaker***  { get; set; }
      * public ConversationSelector ***ConversationSelector***  { get; }
      * public ConversationStarter ***ConversationStarter***  { get; }
      * public Conversation ***NextConversation***  { get; }
   * Methods: 
      * public virtual void ***StartConversation***(Conversation conversation)
      * public virtual void ***StartConversation***()
1. public static class **SpeakerControllersManager**
1. public class **SpeakerPortraitCollection**
   * Methods: 
      * public Sprite ***GetPortrait***(PortraitType type)
### Speakers. U I
1. public class **SpeakerControllerPortraitImage** : MonoBehaviour
### Starters
1. public abstract class **ConversationStarter** : MonoBehaviour
   * Methods: 
      * public abstract ConversationPlayer ***StartConversation***(ISpeaker speaker, Conversation conversation)
1. public class **ConversationStarterFromPrefab** : ConversationStarter
   * Methods: 
      * public override ConversationPlayer ***StartConversation***(ISpeaker speaker, Conversation conversation)
1. public class **ConversationStarterFromResource** : ConversationStarter
   * Methods: 
      * public override ConversationPlayer ***StartConversation***(ISpeaker speaker, Conversation conversation)
### U I
1. public class **DialogueLineImage** : MonoBehaviour
1. public class **DialogueLineResizer** : MonoBehaviour
1. public class **SpeakerBackgroundColor** : MonoBehaviour
1. public class **SpeakerNameText** : MonoBehaviour
1. public class **SpeakerPortraitImage** : MonoBehaviour
### U I. Responses
1. public class **DialogueResponseBlock** : MonoBehaviour
   * Methods: 
      * public virtual DialogueResponseBlock ***Initialize***(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
      * public void ***Submit***(BasicResponse response)
      * public void ***Destroy***()
1. public class **DialogueResponseButton** : MonoBehaviour
   * Actions: 
      * public event Action ***OnResponseChanged*** 
   * Properties: 
      * public BasicResponse ***Response***  { get; }
   * Methods: 
      * public virtual void ***Initialize***(DialogueResponseBlock responseBlock, BasicResponse response)
1. public class **DialogueResponseButtonOutline** : MonoBehaviour
1. public class **DialogueResponseButtonText** : MonoBehaviour
1. public class **DialogueResponseGridBlock** : DialogueResponseBlock
   * Methods: 
      * public override DialogueResponseBlock ***Initialize***(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
### U I. Speech Bubble Tail
1. public class **SpeechBubbleAnchor** : MonoBehaviour
   * Properties: 
      * public SpeakerController ***SpeakerController***  { get; }
1. public static class **SpeechBubbleAnchorsManager**
1. public class **SpeechBubbleTail** : Graphic
### U I. Text Animation
1. public abstract class **DialogueLineTyper** : MonoBehaviour
   * Properties: 
      * public Boolean ***IsAnimating***  { get; }
   * Methods: 
      * public abstract void ***ForceFinishAnimating***()
1. public class **DialogueLineTyperTMPro** : DialogueLineTyper
   * Methods: 
      * public override void ***ForceFinishAnimating***()
1. public class **DialogueLineTyperText** : DialogueLineTyper
   * Methods: 
      * public override void ***ForceFinishAnimating***()
