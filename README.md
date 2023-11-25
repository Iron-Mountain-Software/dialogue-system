# Dialogue System
*Version: 2.6.0*
## Description: 
A system for writing and playing branching dialogue.
## Dependencies: 
* com.unity.localization (1.3.2)
* com.unity.textmeshpro (3.0.6)
* com.github.siccity.xnode (1.8.0)
* com.iron-mountain.save-system (1.0.4)
* com.iron-mountain.conditions (1.5.0)
* com.iron-mountain.resource-utilities (1.1.2)
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
      * public String ***InvokingLine***  { get; }
      * public Sprite ***InvokingIcon***  { get; }
      * public Boolean ***AlertInConversationMenu***  { get; }
      * public ConversationPreviewType ***PreviewType***  { get; }
      * public String ***PreviewText***  { get; }
      * public Condition ***Condition***  { get; set; }
      * public BehaviorWhenQueued ***BehaviorWhenEnqueued***  { get; }
      * public Boolean ***Looping***  { get; }
      * public Boolean ***IsActive***  { get; set; }
      * public Int32 ***Playthroughs***  { get; set; }
      * public Boolean ***GeneralSectionHasErrors***  { get; }
      * public Boolean ***PreviewHasErrors***  { get; }
      * public Boolean ***ConditionHasErrors***  { get; }
   * Methods: 
      * public void ***RefreshActiveState***()
      * public virtual void ***OnConversationStarted***()
      * public virtual void ***Reset***()
      * public void ***GenerateNewID***()
      * public Boolean ***HasWarnings***()
      * public Boolean ***HasErrors***()
      * public Boolean ***GraphHasErrors***()
      * public void ***LogGraphErrors***()
1. public enum **ConversationPreviewType** : Enum
1. public class **ConversationsManager** : MonoBehaviour
1. public class **DialogueLine**
   * Properties: 
      * public ISpeaker ***Speaker***  { get; }
      * public String ***Text***  { get; }
      * public AudioClip ***AudioClip***  { get; }
      * public PortraitType ***Portrait***  { get; }
      * public AnimationType ***Animation***  { get; }
      * public Sprite ***Sprite***  { get; }
1. public class **DialogueTouchInputManager** : MonoBehaviour
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
1. public class **DialogueNarration** : MonoBehaviour
   * Methods: 
      * public void ***RefreshRequirements***()
1. public abstract class **DialogueNarrationRequirement** : MonoBehaviour
   * Methods: 
      * public abstract Boolean ***IsSatisfied***()
### Nodes
1. public struct **Connection**
1. public class **DialogueBeginningNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
1. public class **DialogueEndingNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
1. public class **DialogueLineMainContent**
   * Properties: 
      * public LocalizedString ***TextData***  { get; }
      * public String ***Text***  { get; }
      * public AudioClip ***AudioClip***  { get; }
1. public class **DialogueLineNode** : DialogueNode
   * Properties: 
      * public SpeakerType ***SpeakerType***  { get; }
      * public Speaker ***CustomSpeaker***  { get; }
      * public String ***Text***  { get; }
      * public LocalizedString ***LocalizedText***  { get; }
      * public AudioClip ***AudioClip***  { get; }
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
1. public class **DialogueLineWithAlternatesNode** : DialogueLineNode
   * Properties: 
      * public List<DialogueLineMainContent> ***AlternateContent***  { get; }
1. public abstract class **DialogueNode** : Node
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public abstract DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public virtual void ***OnNodeEnter***(ConversationPlayer conversationUI)
      * public virtual void ***OnNodeExit***(ConversationPlayer conversationUI)
      * public DialogueNode ***GetNextHaltingNode***(ConversationPlayer conversationUI)
      * public Boolean ***HasWarnings***()
      * public Boolean ***HasErrors***()
1. public class **DialoguePassNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
1. public class **DialogueRandomSelectorNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
1. public class **DialogueResponseBlockNode** : DialogueNode
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public List`1 ***GetResponseGenerators***()
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
      * public override void ***OnNodeExit***(ConversationPlayer conversationUI)
      * public override void ***OnCreateConnection***(NodePort from, NodePort to)
### Nodes. Actions
1. public abstract class **DialogueAction** : DialogueNode
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
      * public void ***LogErrors***()
1. public class **DialoguePlayPrioritized** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
1. public class **LoadScene** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
1. public class **UnityEvent** : DialogueAction
   * Properties: 
      * public String ***Name***  { get; }
### Nodes. Conditions
1. public abstract class **Condition** : DialogueNode
   * Methods: 
      * public void ***LogErrors***()
1. public abstract class **PassFailCondition** : Condition
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
      * public override void ***OnNodeExit***(ConversationPlayer conversationUI)
1. public class **PassFailConditionDialogueQueued** : PassFailCondition
   * Properties: 
      * public String ***Name***  { get; }
1. public class **PassFailConditionFromReference** : PassFailCondition
   * Properties: 
      * public String ***Name***  { get; }
### Nodes. Response Generators
1. public abstract class **ResponseGenerator** : DialogueNode
   * Properties: 
      * public ScriptedResponseStyle ***ScriptedResponseStyle***  { get; }
   * Methods: 
      * public abstract List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
      * public override DialogueNode ***GetNextNode***(ConversationPlayer conversationUI)
      * public override void ***OnNodeEnter***(ConversationPlayer conversationUI)
      * public override void ***OnNodeExit***(ConversationPlayer conversationUI)
      * public override void ***OnCreateConnection***(NodePort from, NodePort to)
1. public class **ResponseGeneratorActiveDialogue** : ResponseGenerator
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
1. public class **ResponseGeneratorText** : ResponseGenerator
   * Properties: 
      * public String ***Name***  { get; }
      * public String ***Text***  { get; }
   * Methods: 
      * public override List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
1. public class **ResponseGeneratorTextChat** : ResponseGeneratorText
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
1. public class **ResponseGeneratorTextNeverMind** : ResponseGeneratorText
   * Properties: 
      * public String ***Name***  { get; }
   * Methods: 
      * public override List`1 ***GetDialogueResponses***(ConversationPlayer conversationPlayer)
### Responses
1. public class **BasicResponse**
   * Properties: 
      * public ConversationPlayer ***ConversationPlayer***  { get; }
      * public DialogueNode ***SourceNode***  { get; }
      * public String ***Text***  { get; }
      * public Sprite ***Icon***  { get; }
      * public Int32 ***Row***  { get; }
      * public Int32 ***Column***  { get; }
      * public IResponseStyle ***Style***  { get; }
   * Methods: 
      * public virtual void ***ExecuteResponse***()
1. public interface **IResponseStyle**
   * Properties: 
      * public float ***Height***  { get; }
      * public Color ***ButtonColorPrimary***  { get; }
      * public Color ***ButtonColorSecondary***  { get; }
      * public Color ***TextColor***  { get; }
1. public class **PlayConversationResponse** : BasicResponse
   * Methods: 
      * public override void ***ExecuteResponse***()
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
      * public Color ***Color***  { get; }
      * public Conversation ***DefaultConversation***  { get; }
      * public List<Conversation> ***Conversations***  { get; }
      * public SpeakerPortraitCollection ***Portraits***  { get; }
      * public SpeakerPortraitCollection ***FullBodyPortraits***  { get; }
1. public class **PlayConversationOnPointerClick** : MonoBehaviour
   * Methods: 
      * public virtual void ***OnPointerClick***(PointerEventData eventData)
1. public class **Speaker** : ScriptableObject
   * Actions: 
      * public event Action ***OnActiveConversationsChanged*** 
   * Properties: 
      * public String ***ID***  { get; }
      * public String ***SpeakerName***  { get; }
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
1. public enum **SpeakerType** : Enum
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
1. public class **ConversationPlayer** : MonoBehaviour
   * Actions: 
      * public event Action ***OnDefaultSpeakerChanged*** 
      * public event Action ***OnConversationChanged*** 
      * public event Action ***OnDialogueLinePlayed*** 
      * public event Action ***OnClosed*** 
   * Properties: 
      * public Int32 ***FrameOfLastProgression***  { get; }
      * public float ***TimeOfLastProgression***  { get; }
      * public ISpeaker ***DefaultSpeaker***  { get; }
      * public Conversation ***CurrentConversation***  { get; }
      * public DialogueNode ***CurrentNode***  { get; set; }
      * public DialogueLine ***CurrentDialogueLine***  { get; set; }
   * Methods: 
      * public ConversationPlayer ***Initialize***(ISpeaker speaker, Conversation conversation)
      * public void ***Close***()
      * public void ***PlayDialogueLine***(DialogueLine dialogueLine)
      * public void ***GenerateResponseBlock***(DialogueResponseBlockNode dialogueResponseBlockNode)
      * public void ***DestroyResponseBlock***()
      * public void ***PlayNextDialogueNode***()
      * public void ***CompleteConversation***()
1. public static class **ConversationPlayersManager**
1. public class **DialogueLineImage** : MonoBehaviour
1. public class **DialogueLineResizer** : MonoBehaviour
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
1. public class **DialogueResponseBlock** : MonoBehaviour
   * Methods: 
      * public void ***Initialize***(DialogueResponseBlockNode dialogueResponseBlock, ConversationPlayer conversationUI)
      * public void ***Destroy***()
1. public class **DialogueResponseButton** : MonoBehaviour
   * Actions: 
      * public event Action ***OnBasicResponseChanged*** 
   * Properties: 
      * public BasicResponse ***BasicResponse***  { get; }
   * Methods: 
      * public virtual void ***Initialize***(BasicResponse basicResponse, ConversationPlayer conversationUI)
      * public void ***OnClick***()
1. public class **DialogueResponseButtonOutline** : MonoBehaviour
1. public class **SpeakerBackgroundColor** : MonoBehaviour
1. public class **SpeakerNameText** : MonoBehaviour
1. public class **SpeakerPortraitImage** : MonoBehaviour
1. public class **UI_DialogueResponseWithIcon** : DialogueResponseButton
   * Methods: 
      * public override void ***Initialize***(BasicResponse basicResponse, ConversationPlayer conversationUI)
### U I. Speech Bubble Tail
1. public class **SpeechBubbleAnchor** : MonoBehaviour
   * Properties: 
      * public SpeakerController ***SpeakerController***  { get; }
1. public static class **SpeechBubbleAnchorsManager**
1. public class **SpeechBubbleTail** : Graphic
