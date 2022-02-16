using Newtonsoft.Json;

namespace DialogueSystem
{
    public class Dialogue
    {
        public Dialogue()
        { }
        public Dialogue(string name, string lang)
        {
            Name = name;
            Language = lang;
        }

        public Dialogue(string name, string lang, Talk FirstTalk) : this(name, lang)
        {
            this.FirstTalk = FirstTalk;
        }
        [JsonProperty]
        public string Name = "-";
        [JsonProperty]
        public string Language { get; internal set; } = "-";
        [JsonIgnore]
        public List<Condition> dialogueConditions { get { return GetCondition(FirstTalk, true); } }
        [JsonProperty]
        public List<RuntimeVariable> runtimeVariables = new List<RuntimeVariable>();

        [JsonProperty]
        public Talk FirstTalk { get; internal set; } 
        [JsonIgnore]
        internal bool endReached = false;
        [JsonIgnore]
        public bool EndReached { get { return endReached; } }
        [JsonIgnore]
        internal Talk? currentTalk = null;
        [JsonIgnore]
        public Talk? CurrentTalk
        {
            get
            {
                if (currentTalk == null && !endReached) currentTalk = FirstTalk;
                if (currentTalk != null && currentTalk.hasAppliedInternalModifiers == false) currentTalk.ApplyInternalModifier(this); return currentTalk;
            }
        }
        [JsonIgnore]
        private List<Talk> cleared = new List<Talk>();

        private List<Condition> GetCondition(Talk talk, bool firstCall = true)
        {
            List<Condition> condition = new List<Condition>();

            if (firstCall) cleared.Clear();
            if (talk == null || cleared.Contains(talk)) return condition; //empty list
            else
            {
                cleared.Add(talk);
                
                if (talk.HasCondition) condition.Add(talk.TalkCondition); //add talk condition
                if (talk.HasAnswers)
                {
                    foreach (Answer a in talk.answers)
                    {
                        if (a.HasCondition)
                        {
                            condition.Add(a.AnswerCondition); //add answer condition
                        }
                        if (a.UnconditionalNext != null) condition.AddRange(GetCondition(a.UnconditionalNext, false)); //recursive
                        if (a.HasCondition)
                        {
                            if (a.AnswerCondition.TalkIfTrue != null) condition.AddRange(GetCondition(a.AnswerCondition.TalkIfTrue, false)); //recursive
                            if (a.AnswerCondition.TalkIfFalse != null) condition.AddRange(GetCondition(a.AnswerCondition.TalkIfFalse, false)); //recursive
                        }
                    }
                }
                if (talk.UnconditionalNext != null) condition.AddRange(GetCondition(talk.UnconditionalNext, false)); //recursive
                if (talk.HasCondition)
                {
                    if (talk.TalkCondition.TalkIfTrue != null) condition.AddRange(GetCondition(talk.TalkCondition.TalkIfTrue, false)); //recursive
                    if (talk.TalkCondition.TalkIfFalse != null) condition.AddRange(GetCondition(talk.TalkCondition.TalkIfFalse, false)); //recursive
                }
                
                return condition;
            }
        }

        public List<DialogueAction>? NextStep() //with no answers
        {
            if (!endReached && currentTalk == null) currentTalk = FirstTalk;
            if (endReached) return null;
            else
            {
                if (currentTalk != null && currentTalk.answers.Count != 0) throw new Exception($"Answer not specified to {currentTalk.tag} Talk");
                else
                {
                    List<DialogueAction> toReturn = new List<DialogueAction>(); toReturn.AddRange(currentTalk.UnconditionalExternalAction);
                    List<InternalAction> toExecute = new List<InternalAction>(); toExecute.AddRange(currentTalk.UnconditionalInternalActions);
                    if (currentTalk.HasCondition) { toReturn.AddRange(currentTalk.TalkCondition.GetExternalActions()); toExecute.AddRange(currentTalk.TalkCondition.GetInternalActions()); }

                    foreach (InternalAction action in toExecute) action.Execute(this);

                    if (currentTalk.HasCondition) currentTalk = currentTalk.TalkCondition.GetNextTalk();
                    else if(currentTalk.HasCondition == false) currentTalk = currentTalk.UnconditionalNext;
                    if (currentTalk == null) endReached = true;
                    return toReturn;
                }
            }
        }

        public List<DialogueAction>? NextStep(int answerIndex = 0)
        {
            if (!endReached && currentTalk == null) currentTalk = FirstTalk;
            if (endReached) return null;
            else
            {
                if (currentTalk != null && answerIndex < currentTalk.answers.Count && currentTalk.answers[answerIndex] != null)
                {
                    List<DialogueAction> toReturn = new List<DialogueAction>(); toReturn.AddRange(currentTalk.UnconditionalExternalAction); toReturn.AddRange(currentTalk.answers[answerIndex].ExternalAction);
                    List<InternalAction> toExecute = new List<InternalAction>(); toExecute.AddRange(currentTalk.UnconditionalInternalActions); toExecute.AddRange(currentTalk.answers[answerIndex].InternalActions);
                    if (currentTalk.HasCondition) { toReturn.AddRange(currentTalk.TalkCondition.GetExternalActions()); toExecute.AddRange(currentTalk.TalkCondition.GetInternalActions()); }
                    if (currentTalk.answers[answerIndex].HasCondition) { toReturn.AddRange(currentTalk.answers[answerIndex].AnswerCondition.GetExternalActions()); toExecute.AddRange(currentTalk.answers[answerIndex].AnswerCondition.GetInternalActions()); }

                    foreach (InternalAction action in toExecute) action.Execute(this);

                    if (currentTalk.answers[answerIndex].HasCondition) currentTalk = currentTalk.answers[answerIndex].AnswerCondition.GetNextTalk();
                    else if (currentTalk.answers[answerIndex].HasCondition == false) currentTalk = currentTalk.answers[answerIndex].UnconditionalNext;
                    if (currentTalk == null) endReached = true;
                    return toReturn;
                }
                else throw new NullReferenceException($"The answer given in {currentTalk.tag} Talk ({answerIndex}) is out of range (max = {currentTalk.answers.Count - 1}) or an answer is null");
            }
        }
    }

    public class Talk
    {
        public Talk() { }
        public Talk(string text, string tag, string? characterName = null)
        {
            this.text = text;
            this.tag = tag;
            this.characterName = characterName;
        }

        public Talk(string text, string tag, string? characterName, List<Answer> answers) : this(text, tag, characterName)
        {
            this.answers = answers;
        }

        public Talk(string text, string tag, string? characterName, params Answer[] inputAnswers) : this(text, tag, characterName)
        {
            foreach (Answer answer in inputAnswers) answers.Add(answer);
        }

        public Talk(string text, string tag, string? characterName, List<Answer>? answers, List<Modifier>? modifiers) : this(text, tag, characterName)
        {
            if (answers != null) this.answers = answers;
            if (modifiers != null) this.modifiers = modifiers;
        }

        public Talk(string text, string tag, string? characterName, List<Answer>? answers, List<Modifier>? modifiers, List<DialogueAction>? actions) : this(text, tag, characterName, answers, modifiers)
        {
            if (actions != null) this.unconditionalActions = actions;
        }

        public Talk(string text, string tag, string? characterName, List<Answer>? answers, List<Modifier>? modifiers, params DialogueAction[] inputActions) : this(text, tag, characterName, answers, modifiers)
        {
            foreach (DialogueAction act in inputActions) unconditionalActions.Add(act);
        }

        public void ApplyInternalModifier(Dialogue parent)
        {
            if (hasAppliedInternalModifiers == false)
            {
                foreach (InternalModifier internalMod in InternalModifier)
                {
                    internalMod.Execute(parent, ref text);
                }
                hasAppliedInternalModifiers = true;
            }
        }
        [JsonProperty]
        public string text = "-";
        [JsonProperty]
        public string tag { get; private set; } = "-";
        [JsonProperty]
        public string? characterName = null;
        [JsonProperty]
        public List<Answer> answers = new List<Answer>();
        [JsonProperty]
        public List<Modifier> modifiers = new List<Modifier>();
        [JsonProperty]
        public List<DialogueAction> unconditionalActions = new List<DialogueAction>();
        [JsonProperty]
        public Condition? TalkCondition { get; set; }
        [JsonIgnore]
        public bool HasCondition { get { return TalkCondition != null; } }
        [JsonIgnore]
        internal bool hasAppliedInternalModifiers = false;
        [JsonIgnore]
        public bool HasAnswers { get { return answers.Count > 0; } }
        [JsonProperty]
        public Talk? UnconditionalNext { get; set; }

        [JsonIgnore]
        public List<InternalAction> UnconditionalInternalActions
        {
            get
            {
                List<InternalAction> toReturn = new List<InternalAction>();
                foreach (DialogueAction action in unconditionalActions) if (action is InternalAction) toReturn.Add((InternalAction)action);
                return toReturn;
            }
        }
        [JsonIgnore]
        public List<DialogueAction> UnconditionalExternalAction
        {
            get
            {
                List<DialogueAction> toReturn = new List<DialogueAction>();
                foreach (DialogueAction action in unconditionalActions) if (!(action is InternalAction)) toReturn.Add(action);
                return toReturn;
            }
        }
        [JsonIgnore]
        public List<InternalModifier> InternalModifier
        {
            get
            {
                List<InternalModifier> toReturn = new List<InternalModifier>();
                foreach (Modifier mod in modifiers) if (mod is InternalModifier) toReturn.Add((InternalModifier)mod);
                return toReturn;
            }
        }
        [JsonIgnore]
        public List<Modifier> ExternalModifier
        {
            get
            {
                List<Modifier> toReturn = new List<Modifier>();
                foreach (Modifier mod in modifiers) if (!(mod is InternalModifier)) toReturn.Add(mod);
                return toReturn;
            }
        }
    }

    public class Answer
    {
        public Answer() { }
        public Answer(string text)
        {
            this.text = text;
        }
        public Answer(string text, List<DialogueAction> actions) : this(text)
        {
            this.actions = actions;
        }

        public Answer(string text, params DialogueAction[] inputActions) : this(text)
        {
            foreach (DialogueAction action in inputActions) actions.Add(action);
        }
        [JsonProperty]
        public string text = "-";
        [JsonProperty]
        public List<DialogueAction> actions = new List<DialogueAction>();
        [JsonProperty]
        public Condition? AnswerCondition { get; set; }
        [JsonIgnore]
        public bool HasCondition { get { return AnswerCondition != null; } }
        [JsonProperty]
        public Talk? UnconditionalNext;

        [JsonIgnore]
        public List<InternalAction> InternalActions
        {
            get
            {
                List<InternalAction> toReturn = new List<InternalAction>();
                foreach (DialogueAction action in actions) if (action is InternalAction) toReturn.Add((InternalAction)action);
                return toReturn;
            }
        }
        [JsonIgnore]
        public List<DialogueAction> ExternalAction
        {
            get
            {
                List<DialogueAction> toReturn = new List<DialogueAction>();
                foreach (DialogueAction action in actions) if (!(action is InternalAction)) toReturn.Add(action);
                return toReturn;
            }
        }
    }

    public abstract class DialogueAction
    {
        public DialogueAction() { }
    }

    public abstract class InternalAction : DialogueAction //action that are handled automatically by the library
    {
        public InternalAction() { }
        public abstract void Execute(Dialogue parent);
    }

    public abstract class Modifier
    {
        public Modifier() { }
        [JsonProperty]
        public int position; //index of the modifier in the text, for example in "How are you? <Stop for 1 second> Im fine" index would be 13 because the modifier has it's effect when the text arrive at 13
    }

    public abstract class InternalModifier : Modifier //modifier that are handled automatically by the library
    {
        public InternalModifier() { }
        public abstract void Execute(Dialogue parent, ref string currentTalkText);
    }

    public class Condition
    {
        internal Condition() //only to be used by newtonsoft.json
        {
            
        }

        public Condition(string name, string argument) :this()
        {
            this.name = name;
            this.argument = argument;
        }

        public Condition(string name, string argument, Talk? talkIfTrue, Talk? talkIfFalse) : this(name, argument)
        {
            this.TalkIfFalse = talkIfFalse;
            this.TalkIfTrue = talkIfTrue;
        }

        [JsonProperty]
        public string name = "-";
        [JsonProperty]
        public string argument = "-";

        public bool? outcome = null;

        [JsonProperty]
        public List<DialogueAction> actionsIfTrue = new List<DialogueAction>();
        [JsonProperty]
        public List<DialogueAction> actionsIfFalse = new List<DialogueAction>();
        [JsonProperty]
        internal Talk? TalkIfTrue { get; set; }
        [JsonProperty]
        internal Talk? TalkIfFalse { get; set; }

        public Talk? GetNextTalk()
        {
            if (outcome == true) return TalkIfTrue;
            else if (outcome == false) return TalkIfFalse;
            else throw new NullReferenceException($"Condition {name},{argument} is not evaluated by the game engine, it should be done before anything else when starting a dialogue");
        }

        public List<DialogueAction> GetExternalActions()
        {
            List<DialogueAction> toReturn = new List<DialogueAction>();
            List<DialogueAction> toCheck;
            if (outcome == null) throw new NullReferenceException($"Condition {name},{argument} is not evaluated, it should be done before anything else when starting a dialogue");
            if (outcome == true) toCheck = actionsIfTrue; else toCheck = actionsIfFalse;
            foreach (DialogueAction action in toCheck) if (!(action is InternalAction)) toReturn.Add(action);
            return toReturn;
        }

        public List<InternalAction> GetInternalActions()
        {
            List<InternalAction> toReturn = new List<InternalAction>();
            List<DialogueAction> toCheck;
            if (outcome == null) throw new NullReferenceException($"Condition {name},{argument} is not evaluated by the game engine, it should be done before anything else when starting a dialogue");
            if (outcome == true) toCheck = actionsIfTrue; else toCheck = actionsIfFalse;
            foreach (DialogueAction action in toCheck) if (action is InternalAction) toReturn.Add((InternalAction)action);
            return toReturn;
        }
    }

    public class RuntimeVariable
    {
        public RuntimeVariable()
        {
           
        }

        public RuntimeVariable(string Name) : this()
        {
            this.Name = Name;
        }

        public RuntimeVariable(string Name, string Value) : this()
        {
            this.Name = Name;
            this.Value = Value;
        }
        [JsonProperty]
        public string? Name { get; internal set; }
        [JsonProperty]
        public string? Value { get; set; }
    }
}