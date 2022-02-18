using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogueSystem
{
    public class GenericEngineHandledAction : DialogueAction
    {
        public GenericEngineHandledAction() { }

        public GenericEngineHandledAction(string name, string value)
        {
            this.name = name; this.value = value; 
        }

        public string name = "-";
        public string value = "-";
    }

    /*
    public class EndDialogueAction : InternalAction //usless action (a dialogue automatically ends when the next talk is null) created just to test the internal action functionality
    {
        public EndDialogueAction() { }
        public override void Execute(Dialogue parent)
        {
            parent.endReached = true;
        }
    }
    */
}
