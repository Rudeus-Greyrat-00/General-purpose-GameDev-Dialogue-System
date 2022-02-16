using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DialogueSystem
{
    public static class DialogueManager
    { 
        /*
        public static Dialogue? JSONParse(string value)
        {
            if (JsonConvert.DeserializeObject<Dialogue>(value) != null) return JsonConvert.DeserializeObject<Dialogue>(value);
            else throw new Exception("JSONParse returned null");
        }
        */

        
        public static Dialogue JSONParse(string value)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.TypeNameHandling = TypeNameHandling.All;

            TextReader reader = new StringReader(value);
            JsonReader JSONreader = new JsonTextReader(reader);

            Dialogue? dialogue = (Dialogue)serializer.Deserialize(JSONreader);
            if (dialogue != null) return dialogue;
            else throw new Exception("JSON deserializer returned null");
        }
        
    }

    public static class DialogueUtilities
    {
        public static string? ConvertToJSONString(Dialogue dialogue)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.TypeNameHandling = TypeNameHandling.All;

            TextWriter textWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, dialogue);

            return textWriter.ToString();
        }
        public static void ConvertToJSON(Dialogue dialogue, string destinationPath, string fileName)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.TypeNameHandling = TypeNameHandling.All;

            using (StreamWriter sw = new StreamWriter(@$"{destinationPath}\{fileName}.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, dialogue);
            }
        }

        public static void ConvertToEDP(Dialogue dialogue, string destinationFileName)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.TypeNameHandling = TypeNameHandling.All;

            using (StreamWriter sw = new StreamWriter(@$"{destinationFileName}"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, dialogue);
            }
        }
    }

    public static class DialogueDebug
    {
        public static Dialogue GenerateTestDialogue()
        {
            //EndDialogueAction endDialogueAction = new EndDialogueAction();

            GenericEngineHandledAction act1 = new GenericEngineHandledAction("act", "actval");
            GenericEngineHandledAction act2 = new GenericEngineHandledAction("act", "actval");
            GenericEngineHandledAction act3 = new GenericEngineHandledAction("act", "actval");

            Answer answer1 = new Answer("yes it is beautiful");
            answer1.actions.Add(act1);
            
            Answer answer2 = new Answer("no i don't like that");
            answer2.actions.Add(act2);

            Condition condition1 = new Condition("hasLevelGreaterThan", "30");
            Condition condition2 = new Condition("hasItem", "EmeraldSword");
            Condition condition3 = new Condition("hasDoneQuest", "FindTheTreasure1");

            LoadDialogueRuntimeVariableModifier sunComment = new LoadDialogueRuntimeVariableModifier(25,"value1");
            LoadDialogueRuntimeVariableModifier skyComment = new LoadDialogueRuntimeVariableModifier(25,"value2");

            List<Modifier> mod = new List<Modifier>();
            mod.Add(sunComment); mod.Add(skyComment);

            Talk talk1 = new Talk("today is a beautiful day ", "statment", null, null, mod);
            Talk talk2 = new Talk("Isn't it?", "question", null, answer1, answer2);
            Talk talk3 = new Talk("I'm glad that you appreciate this too!", "statement if player chosed yes");
            Talk talk4 = new Talk("What!? Why you don't like that!?", "statement if player chosed no");
            Talk talk5 = new Talk("Anyway now ther's not time to waste, keep going straiwfoard and continue your adventure!!", "final statement");
            talk1.unconditionalActions.Add(act3);

            condition2.TalkIfTrue = new Talk("oh and you have a really beautiful sword!", "comment");
            condition2.TalkIfFalse = new Talk("oh and i think you should really get a sword to go this way...", "comment2");

            talk1.UnconditionalNext = talk2;
            answer1.UnconditionalNext = talk3;
            answer2.UnconditionalNext = talk4;
            talk4.UnconditionalNext = talk5;
            talk4.TalkCondition = condition2;

            Dialogue testDialogue = new Dialogue("TestDialogue", "Eng", talk1);

            testDialogue.runtimeVariables.Add(new RuntimeVariable("value1", "Look at that sun!"));
            testDialogue.runtimeVariables.Add(new RuntimeVariable("value2", "The sky is so bright..."));

            return testDialogue;
        }
    }
}
