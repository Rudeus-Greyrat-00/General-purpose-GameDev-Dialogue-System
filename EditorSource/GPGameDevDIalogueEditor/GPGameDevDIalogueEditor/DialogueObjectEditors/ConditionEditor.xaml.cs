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
using DialogueSystem;
using Newtonsoft.Json;

namespace GPGameDevDialogueEditor.DialogueObjectEditors
{
    /// <summary>
    /// Logica di interazione per ConditionEditor.xaml
    /// </summary>
    public partial class ConditionEditor : UserControl
    {
        internal TalkEditor? container;
        internal static int HeighValue = 79;
        internal int ActionCount { get { return ActionIfFalseStackPanel.Children.Count + ActionIfTrueStackPanel.Children.Count; } }
        public ConditionEditor()
        {
            InitializeComponent();
            MainWindow.HasUnsavedChange = true;
        }

        public ConditionEditor(string Name, string Argument)
        {
            InitializeComponent();
            CustomFunctionality.TextboxGotFocus(Condition);
            CustomFunctionality.TextboxGotFocus(this.Argument);
            Condition.Text = Name;
            this.Argument.Text = Argument;
            CustomFunctionality.TextboxLostFocus(Condition);
            CustomFunctionality.TextboxLostFocus(this.Argument);
        }

       internal int CalculateHeighValue()
        {
            int toReturn = 0;
            foreach (ActionEditor a in ActionIfTrueStackPanel.Children) toReturn += ActionEditor.HeighValue;
            foreach (ActionEditor a in ActionIfFalseStackPanel.Children) toReturn += ActionEditor.HeighValue;
            toReturn += ConditionEditor.HeighValue;
            return toReturn;
        }

        private void AddTrueConditionAction(object sender, RoutedEventArgs e)
        {
            ActionEditor action = new ActionEditor();
            action.conditionContainer = this;
            UpdateTalkRectangle(ActionEditor.HeighValue);
            ActionIfTrueStackPanel.Children.Add(action);
        }

        private void AddFalseConditionAction(object sender, RoutedEventArgs e)
        {
            ActionEditor action = new ActionEditor();
            action.conditionContainer = this;
            UpdateTalkRectangle(ActionEditor.HeighValue);
            ActionIfFalseStackPanel.Children.Add(action);
        }

        private void UpdateTalkRectangle(int delta)
        {
            if (container != null) { container.RectangleContainer.Height += delta; }
        }

        internal void ActionRemoved()
        {
            UpdateTalkRectangle(-ActionEditor.HeighValue);
        }

        private void RemoveThis(object sender, RoutedEventArgs e)
        {
            int contentCount = 0;
            if (Parent is StackPanel stackPanel)
            {
                stackPanel.Children.Remove(this);
            }
            trueStarter.DeleteLine();
            falseStarter.DeleteLine();
            contentCount = ActionIfFalseStackPanel.Children.Count + ActionIfTrueStackPanel.Children.Count;
            UpdateTalkRectangle(-(HeighValue + ActionEditor.HeighValue * contentCount));
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                CustomFunctionality.TextboxGotFocus(textbox);
            }
            else throw new Exception("This function must be called only by text box");
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                CustomFunctionality.TextboxLostFocus(textbox);
            }
            else throw new Exception("This function must be called only by text box");
        }

        public DialogueSystem.Condition Export()
        {
            Talk? nextIfTrue = null, nextIfFalse = null;
            if (trueStarter.NextTalk != null) nextIfTrue = trueStarter.NextTalk.Export();
            if(falseStarter.NextTalk != null) nextIfFalse = falseStarter.NextTalk.Export();
            DialogueSystem.Condition condition = new DialogueSystem.Condition(Condition.Text, Argument.Text, nextIfTrue, nextIfFalse);
            foreach (UIElement el in ActionIfTrueStackPanel.Children) if (el is ActionEditor ac) condition.actionsIfTrue.Add(ac.Export());
            foreach (UIElement el in ActionIfFalseStackPanel.Children) if (el is ActionEditor ac) condition.actionsIfFalse.Add(ac.Export());
            return condition;
        }

        public ConditionEditorData ExportData()
        {
            List<ActionEditorData> actionIfTrue = new List<ActionEditorData>();
            List<ActionEditorData> actionIfFalse = new List<ActionEditorData>();
            foreach (UIElement el in ActionIfTrueStackPanel.Children) if (el is ActionEditor ac) actionIfTrue.Add(ac.ExportData());
            foreach (UIElement el in ActionIfFalseStackPanel.Children) if (el is ActionEditor ac) actionIfFalse.Add(ac.ExportData());
            Int32? nextIfTrue = null, nextIfFalse = null;
            if (trueStarter.NextTalk != null) nextIfTrue = trueStarter.NextTalk.Hash();
            if(falseStarter.NextTalk != null) nextIfFalse = falseStarter.NextTalk.Hash();
            return new ConditionEditorData(Condition.Text, Argument.Text, nextIfTrue, nextIfFalse, actionIfTrue, actionIfFalse);
        }

        public static ConditionEditor ImportData(ConditionEditorData data, TalkEditor container)
        {
            ConditionEditor condition = new ConditionEditor(data.Name, data.Argument);
            condition.container = container;
            foreach (ActionEditorData ac in data.actionsIfTrue)
            {
                ActionEditor actionEditor = ActionEditor.ImportData(ac);
                actionEditor.conditionContainer = condition;
                condition.ActionIfTrueStackPanel.Children.Add(actionEditor); 
            }
            foreach (ActionEditorData ac in data.actionIfFalse) 
            {
                ActionEditor actionEditor = ActionEditor.ImportData(ac);
                actionEditor.conditionContainer = condition;
                condition.ActionIfFalseStackPanel.Children.Add(actionEditor);
            }

            if (data.nextIfTrue != null && TalkEditor.ExistInCanvas((Int32)data.nextIfTrue)) condition.trueStarter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.nextIfTrue);
            else if (data.nextIfTrue != null) { CustomFunctionality.SpawnTalkInCanvas((Int32)data.nextIfTrue); condition.trueStarter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.nextIfTrue); }

            if (data.nextIfFalse != null && TalkEditor.ExistInCanvas((Int32)data.nextIfFalse)) condition.falseStarter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.nextIfFalse);
            else if (data.nextIfFalse != null) { CustomFunctionality.SpawnTalkInCanvas((Int32)data.nextIfFalse); condition.falseStarter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.nextIfFalse); }

            return condition;
        }
    }

    public class ConditionEditorData
    {
        [JsonProperty]
        public string Name = "";
        [JsonProperty]
        public string Argument = "";
        [JsonProperty]
        public List<ActionEditorData> actionsIfTrue = new List<ActionEditorData>();
        [JsonProperty]
        public List<ActionEditorData> actionIfFalse = new List<ActionEditorData>();
        [JsonProperty]
        public Int32? nextIfTrue;
        [JsonProperty]
        public Int32? nextIfFalse;

        public ConditionEditorData()
        {

        }

        public ConditionEditorData(string Name, string Argument, Int32? nextTrue, Int32? nextFalse,  List<ActionEditorData>? INactionIfTrue = null, List<ActionEditorData>? INactionIfFalse = null)
        {
            this.Name = Name; this.Argument = Argument; this.nextIfTrue = nextTrue; this.nextIfFalse = nextFalse;
            if (INactionIfTrue != null) this.actionsIfTrue = INactionIfTrue;
            if(INactionIfFalse != null) this.actionIfFalse = INactionIfFalse;
        }

    }
}
