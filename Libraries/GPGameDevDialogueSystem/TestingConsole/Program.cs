using DialogueSystem;
// See https://aka.ms/new-console-template for more information
Dialogue mainTest = DialogueDebug.GenerateTestDialogue();
//DialogueUtilities.ConvertToJSON(mainTest, @"C:\Users\ksamu\OneDrive\Desktop", "convertedDialogue");

//string text = System.IO.File.ReadAllText(@"C:\Users\ksamu\OneDrive\Desktop\convertedDialogue.json");
//Dialogue testDialogue = DialogueManager.JSONParse(text);
Dialogue testDialogue = DialogueDebug.GenerateTestDialogue();

List<Condition> cond = testDialogue.dialogueConditions;
foreach (Condition c in cond) c.outcome = false;
List<DialogueAction> actions = new List<DialogueAction>();

while (!testDialogue.EndReached)
{
    
    Console.WriteLine(testDialogue.CurrentTalk.text);
    if (testDialogue.CurrentTalk.HasAnswers) 
    {
        foreach (Answer ans in testDialogue.CurrentTalk.answers) Console.WriteLine($">> {ans.text}");
        int ansIndex = Convert.ToInt32(Console.ReadLine());
        actions = testDialogue.NextStep(ansIndex);
    } 
    else actions = testDialogue.NextStep();

    foreach(DialogueAction act in actions)
    {
        Console.WriteLine("found action");
    }
}






