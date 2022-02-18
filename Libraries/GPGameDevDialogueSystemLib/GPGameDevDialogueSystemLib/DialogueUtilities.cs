using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DialogueSystem
{
    public static class DialogueManager
    {
        private static char StartObjectChar = '«';
        private static char EndObjectChar = '»';

        public static Dialogue GetDialogueFromString(string fileText)
        {
            throw new NotImplementedException();    
        }

        public static string GetStringFromDialogue(Dialogue dialogueToConvert)
        {
            throw new NotImplementedException();
        }

    }

    public static class DialogueUtilities
    {


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
            Talk talk3 = new Talk("I'm glad that you appreciate this too!", "statement if player chosed yes", null);
            Talk talk4 = new Talk("What!? Why you don't like that!?", "statement if player chosed no",null);
            Talk talk5 = new Talk("Anyway now ther's not time to waste, keep going straiwfoard and continue your adventure!!", "final statement",null);
            talk1.unconditionalActions.Add(act3);

            condition2.TalkIfTrue = new Talk("oh and you have a really beautiful sword!", "comment",null);
            condition2.TalkIfFalse = new Talk("oh and i think you should really get a sword to go this way...", "comment2",null);

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
