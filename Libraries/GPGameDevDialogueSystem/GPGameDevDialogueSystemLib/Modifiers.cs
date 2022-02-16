using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DialogueSystem
{
    //External modifiers
    public class CharTimeEditModifier : Modifier
    {
        public CharTimeEditModifier() { }
        public CharTimeEditModifier(int index, float value)
        {
            position = index;
            this.value = value;
        }
        [JsonProperty]
        public float value;
    }

    //Internal modifiers
    public class LoadDialogueRuntimeVariableModifier : InternalModifier //used to load a value from the DialogueInterface.runtimeVariables
    {
        public LoadDialogueRuntimeVariableModifier() { }
        public LoadDialogueRuntimeVariableModifier(int index, string runtimeVariablesName)
        {
            this.position = index;
            this.runtimeVariablesName = runtimeVariablesName;
        }
        [JsonProperty]
        public string runtimeVariablesName = "";

        public override void Execute(Dialogue parent, ref string currentTalkText)
        {
            if (string.IsNullOrEmpty(runtimeVariablesName) || string.IsNullOrWhiteSpace(runtimeVariablesName) || LookForRuntimeVariable(parent) == null) 
                throw new Exception($"Runtime variables key: {runtimeVariablesName} not found in current dialogue runtime variables in DialogueInterface or the modifier field runtimeVariableName is null/empty/white space");
            if (runtimeVariablesName != null)
            {
                string? runtimeVar = LookForRuntimeVariable(parent).Value;
                if (runtimeVar == null) runtimeVar = "";
                currentTalkText = currentTalkText.Insert(position, runtimeVar);
                foreach (Modifier mod in parent.currentTalk.modifiers) if (mod != this && mod.position > this.position) mod.position += runtimeVar.Length; //shift all the other modifier because i've just add stuff to the string
            }
        }

        private RuntimeVariable? LookForRuntimeVariable(Dialogue parent)
        {
            foreach (RuntimeVariable r in parent.runtimeVariables) if (r.Name == runtimeVariablesName) return r;
            return null;
        }
    }

    public class LoadVariableModifier : InternalModifier //used to add custom string to talks
    {
        public LoadVariableModifier() { }
        public LoadVariableModifier(int index, string value)
        {
            this.position = index;
            this.value = value;
        }
        [JsonProperty]
        public string value = "";

        public override void Execute(Dialogue parent, ref string currentTalkText)
        {
            currentTalkText = currentTalkText.Insert(position, value);
            foreach (Modifier mod in parent.currentTalk.modifiers) if (mod != this && mod.position > this.position) mod.position += value.Length; //shift all the other modifier because i've just add stuff to the string
        }
    }
}
