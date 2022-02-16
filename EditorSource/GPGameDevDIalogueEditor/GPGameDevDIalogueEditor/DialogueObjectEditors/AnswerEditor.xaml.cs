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
    /// Logica di interazione per AnswerEditor.xaml
    /// </summary>
    public partial class AnswerEditor : UserControl
    {
        internal TalkEditor? container;
        internal static int HeighValue = 80;

        public AnswerEditor()
        {
            InitializeComponent();
            MainWindow.HasUnsavedChange = true;
        }

        public AnswerEditor(string AnswerText)
        {
            InitializeComponent();
            CustomFunctionality.TextboxGotFocus(this.AnswerText);
            this.AnswerText.Text = AnswerText;
            CustomFunctionality.TextboxLostFocus(this.AnswerText);
        }
        
        public int CalculateHeightValue()
        {
            int toReturn = 0;
            if (AnswerConditionContainer.Children.Count > 0 && AnswerConditionContainer.Children[0] != null && AnswerConditionContainer.Children[0] is ConditionEditor c)
            {
                toReturn += c.CalculateHeighValue();
            }
            toReturn += AnswerEditor.HeighValue;
            foreach (object b in AnswerActionContainer.Children) if (b is ActionEditor ed) toReturn += ActionEditor.HeighValue;
            return toReturn;
        }
        private void AddAction(object sender, RoutedEventArgs e)
        {
            ActionEditor action = new ActionEditor();
            action.answerContainer = this;
            UpdateTalkRectangle(ActionEditor.HeighValue);
            AnswerActionContainer.Children.Add(action);
        }

        private void RemoveThis(object sender, RoutedEventArgs e)
        {
            UpdateTalkRectangle(-CalculateHeightValue());
            if (Parent is StackPanel stackPanel)
            {
                stackPanel.Children.Remove(this);
            }
            starter.DeleteLine();
            if(container != null) container.UpdateLineValidation();
        }

        private void UpdateTalkRectangle(int delta)
        {
            if (container != null) { container.RectangleContainer.Height += delta; container.UpdateLineValidation(); }
        }

        internal void ActionRemoved()
        {
            UpdateTalkRectangle(-ActionEditor.HeighValue);
        }

        private void AddCondition(object sender, RoutedEventArgs e) //it should never be more than one condition
        {
            ConditionEditor condition = new ConditionEditor();
            condition.container = container;
            UpdateTalkRectangle(ConditionEditor.HeighValue);
            AnswerConditionContainer.Children.Add(condition);
            starter.InvalidLine();
        }

        private void RemoveCondition(object sender, RoutedEventArgs e) //it should never be more than one condition
        {
            int counter = 0; foreach (object obj in AnswerConditionContainer.Children) if (obj is ConditionEditor c) { counter += c.ActionCount; c.falseStarter.DeleteLine(); c.trueStarter.DeleteLine(); }
            AnswerConditionContainer.Children.Clear();
            UpdateTalkRectangle(-(ConditionEditor.HeighValue + counter * ActionEditor.HeighValue));
            starter.ValidLine();
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

        public Answer Export()
        {
            Answer answer = new Answer();
            string ttext = AnswerText.Text != "What the player says" ? AnswerText.Text : "";
            answer.text = ttext;
            foreach (UIElement el in AnswerActionContainer.Children) if (el is ActionEditor ed) answer.actions.Add(ed.Export());
            if (AnswerConditionContainer.Children.Count > 0 && AnswerConditionContainer.Children[0] != null && AnswerConditionContainer.Children[0] is ConditionEditor ce) answer.AnswerCondition = ce.Export();
            if(starter.NextTalk != null) answer.UnconditionalNext = starter.NextTalk.Export();
            return answer;
        }

        public AnswerEditorData ExportData()
        {
            List<ActionEditorData> actions = new List<ActionEditorData>();
            foreach (UIElement el in AnswerActionContainer.Children) if (el is ActionEditor ed) actions.Add(ed.ExportData());
            Int32? next = null;
            ConditionEditorData? condition = null;
            if (AnswerConditionContainer.Children.Count > 0 && AnswerConditionContainer.Children[0] != null && AnswerConditionContainer.Children[0] is ConditionEditor ce) condition = ce.ExportData();
            if (starter.NextTalk != null) next = starter.NextTalk.Hash();
            string ttext = AnswerText.Text != "What the player says" ? AnswerText.Text : "";
            return new AnswerEditorData(ttext, next, condition, actions, (bool)IsConditionalCB.IsChecked);
        }

        public static AnswerEditor ImportData(AnswerEditorData data, TalkEditor container)
        {
            AnswerEditor answer = new AnswerEditor(data.AnswerText);
            answer.container = container;
            answer.IsConditionalCB.IsChecked = data.IsConditional; //setting this cause the rectangle container of parent to be increased of ConditionEditor.HeighValue due to the event handler of the checkbox.
            answer.AnswerConditionContainer.Children.Clear(); //clear the new child created by the checkbox event handler
            
            if(data.IsConditional) answer.AnswerConditionContainer.Children.Add(ConditionEditor.ImportData(data.condition, container)); //add eventual condition

            if (answer.IsConditionalCB.IsChecked == true) container.RectangleContainer.Height -= ConditionEditor.HeighValue; //fix the problem described in line 144

            foreach (ActionEditorData actiondata in data.actions) 
            {
                ActionEditor actionEditor = ActionEditor.ImportData(actiondata);
                actionEditor.answerContainer = answer;
                answer.AnswerActionContainer.Children.Add(actionEditor); 
            } //add actions

            if (data.next != null && TalkEditor.ExistInCanvas((Int32)data.next)) answer.starter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.next);
            else if (data.next != null) { CustomFunctionality.SpawnTalkInCanvas((Int32)data.next); answer.starter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.next); }

            return answer;
        }
    }

    public class AnswerEditorData
    {
        [JsonProperty]
        public string AnswerText = "";
        [JsonProperty]
        public ConditionEditorData? condition;
        [JsonProperty]
        public List<ActionEditorData> actions = new List<ActionEditorData>();
        [JsonProperty]
        public Int32? next;
        [JsonProperty]
        public bool IsConditional = false;

        public AnswerEditorData()
        {

        }

        public AnswerEditorData(string AnswerText, Int32? next, ConditionEditorData? condition, List<ActionEditorData>? actionList = null, bool IsConditional = false)
        {
            this.AnswerText = AnswerText;
            this.next = next;
            this.condition = condition;
            if (actionList != null) this.actions = actionList;
            this.IsConditional = IsConditional;
        }
    }
}
