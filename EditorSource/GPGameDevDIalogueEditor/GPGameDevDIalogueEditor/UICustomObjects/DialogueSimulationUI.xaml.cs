using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DialogueSystem;
using System.Diagnostics;

namespace GPGameDevDialogueEditor.UICustomObjects
{
    /// <summary>
    /// Logica di interazione per DialogueSimulationUI.xaml
    /// </summary>
    public partial class DialogueSimulationUI : UserControl
    {
        public DialogueSimulationUI()
        {
            InitializeComponent();
        }
        DispatcherFrame frame = new DispatcherFrame();
        private readonly int IterationsLimit = 2000;

        private enum ExpectedInput
        {
            stringIn,
            positiveInteger,
            boolean
        }

        private ExpectedInput currentExpectedInput = ExpectedInput.stringIn;

        public void SimulateDialogue(DialogueSystem.Dialogue testDialogue)
        {
            frame.Continue = false;
            InputTextBox.Text = null;
            TestDialogueOutput.Text = null;

            List<RuntimeVariable> RVvariables = testDialogue.runtimeVariables;
            List<DialogueSystem.Condition> cond = testDialogue.dialogueConditions;
            List<DialogueAction>? actions = new List<DialogueAction>();

            TestDialogueOutput.Text += "STARTING DIALOGUE SIMULATION...\n\n";

            if(RVvariables.Count > 0)
            {
                currentExpectedInput = ExpectedInput.stringIn;
                TestDialogueOutput.Text += "To test the dialogue, first of all you need to fill all the values of the declared runtime variables\n";
                for (int i = 0; i < RVvariables.Count; i++)
                {
                    TestDialogueOutput.Text += $"\nPlease enter value for runtime variable \'{RVvariables[i].Name}\'";
                    InputTextBox.Text = null;
                    frame = new DispatcherFrame();
                    Dispatcher.PushFrame(frame);
                    RVvariables[i].Value = userInputString;
                    TestDialogueOutput.Text += $" \tEntered ---> [{userInputString}]";
                }
            }

            if(cond != null && cond.Count > 0)
            {
                currentExpectedInput = ExpectedInput.boolean;
                TestDialogueOutput.Text += "To test the dialogue, first of all you need to choose an outcome to all the dialogue conditions\n";
                for(int i = 0; i < cond.Count; i++)
                {
                    TestDialogueOutput.Text += $"\nPlease choose an outcome for condition \'{cond[i].name}\' with argument {cond[i].argument}";
                    InputTextBox.Text = null;
                    frame = new DispatcherFrame();
                    Dispatcher.PushFrame(frame);
                    cond[i].outcome = userInputBool;
                    TestDialogueOutput.Text += $" \tEntered ---> [{userInputBool}]";
                }
            }

            int currentIteration = 0;

            if (testDialogue.FirstTalk == null) { TestDialogueOutput.Text += "\n\nThe dialogue is empty. If it shouldn't, make sure that you have linked the DIALOGUE START component to your first talk"; return; }

            currentExpectedInput = ExpectedInput.positiveInteger;
            while (!testDialogue.EndReached)
            {
                if (currentIteration >= IterationsLimit) 
                {
                    TestDialogueOutput.Text = "ITERATIONS OVERFLOW: \nYour dialogue is probably stuck in an infinite cycle, the only way exiting would be editing the dialogue object at runtime, would be a bad practice, please read the documentation for more informations.";
                    frame.Continue = false;
                    InputTextBox.Text = null;
                    return;
                }
                userInput = 1;
                TestDialogueOutput.Text += Environment.NewLine;
                TestDialogueOutput.Text += $"----------Talk with tag: [{testDialogue.CurrentTalk.tag}]----------\n";
                TestDialogueOutput.Text += Environment.NewLine;
                TestDialogueOutput.Text += $"Character name: [{(testDialogue.CurrentTalk.characterName != null ? testDialogue.CurrentTalk.characterName : "not assigned")}]\n\n";
                TestDialogueOutput.Text += $"Talk text:\n[";
                TestDialogueOutput.Text += testDialogue.CurrentTalk.text;
                TestDialogueOutput.Text += $"]\n";
                if (testDialogue.CurrentTalk.HasAnswers)
                {
                    InputTextBox.Text = null;
                    int i = 0;
                    TestDialogueOutput.Text += "\n\nThere are answer to be given to this talk, please select one of the following (enter the number in the text box below)\n\n";
                    foreach (Answer ans in testDialogue.CurrentTalk.answers) { TestDialogueOutput.Text += ($"\n>>{i + 1}: {ans.text}"); i++; }
                    frame = new DispatcherFrame();
                    Dispatcher.PushFrame(frame);

                    if (userInput > i) { TestDialogueOutput.Text += $"\n\n\nInput {userInput} out of bounds (max is {i}), input will be clamped to {i}."; userInput = i; }
                    TestDialogueOutput.Text += $"\n\nYou choosed \'{testDialogue.CurrentTalk.answers[userInput - 1].text}\'\n\n";
                    actions = testDialogue.NextStep(userInput - 1);
                }
                else actions = testDialogue.NextStep();


                TestDialogueOutput.Text += "\nNextStep() function returned this actions that should be implemented in and performed by your game\n[";
                if(actions != null) foreach(DialogueAction act in actions)
                    {
                        if(act is GenericEngineHandledAction gen)
                        {
                            TestDialogueOutput.Text += $"Action name: {gen.name}, Action argument: {gen.value}\n";
                        }
                    }
                TestDialogueOutput.Text += "]";
                currentIteration++;
            }
            TestDialogueOutput.Text += "\n\n\n----------ENDPOINT REACHED----------";
        }



        private int userInput = 1;
        private string userInputString = "";
        private bool userInputBool = false;



        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) 
            {
                switch (currentExpectedInput)
                {
                    case ExpectedInput.positiveInteger:
                        {
                            if (int.TryParse(InputTextBox.Text, out userInput) && userInput > 0) frame.Continue = false;
                            else InputTextBox.Text = null;
                            break;
                        }
                    case ExpectedInput.stringIn:
                        {
                            userInputString = InputTextBox.Text;
                            frame.Continue = false;
                            break;
                        }
                    case ExpectedInput.boolean:
                        {
                            if(InputTextBox.Text.ToLower() == "true" || InputTextBox.Text.ToLower() == "false" || InputTextBox.Text.ToLower() == "f" || InputTextBox.Text.ToLower() == "t")
                            {
                                if (InputTextBox.Text.ToLower() == "true" || InputTextBox.Text.ToLower() == "t") userInputBool = true;
                                else userInputBool = false;
                                frame.Continue = false;
                            }
                            else
                            {
                                InputTextBox.Text = null;
                            }
                            break;
                        }
                }

            }
        }
    }
}
