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
    /// Logica di interazione per ActionEditor.xaml
    /// </summary>
    public partial class ActionEditor : UserControl
    {
        internal TalkEditor? talkContainer;
        internal AnswerEditor? answerContainer;
        internal ConditionEditor? conditionContainer;
        internal static int HeighValue = 21;
        internal string _actionName { get { return ActionName.Text; } set { ActionName.Text = value; } }
        internal string _actionValue { get { return ActionValue.Text; }set { ActionValue.Text = value; } }
        public ActionEditor()
        {
            InitializeComponent();
            MainWindow.HasUnsavedChange = true;
        }

        public ActionEditor(string Name, string Value)
        {
            InitializeComponent();
            CustomFunctionality.TextboxGotFocus(ActionValue);
            CustomFunctionality.TextboxGotFocus(ActionName);
            ActionName.Text = Name;
            ActionValue.Text = Value;
            CustomFunctionality.TextboxLostFocus(ActionValue);
            CustomFunctionality.TextboxLostFocus(ActionName);
        }

        private void RemoveCustomAction(object sender, RoutedEventArgs e)
        {
            if (Parent is StackPanel)
            {
                StackPanel ParentPanel = (StackPanel)Parent;
                ParentPanel.Children.Remove(this);
            }
            else throw new Exception("Action editor parent must be a stack panel");
            if (talkContainer != null) talkContainer.RectangleContainer.Height -= HeighValue;
            else if (answerContainer != null) answerContainer.ActionRemoved();
            else if (conditionContainer != null) conditionContainer.ActionRemoved();
            
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

        public GenericEngineHandledAction Export()
        {
            return new GenericEngineHandledAction(_actionName, _actionValue);
        }

        public ActionEditorData ExportData()
        {
            string name = _actionName != "Action Name" ? _actionName : "";
            string value = _actionValue != "Action Value" ? _actionValue : "";
            return new ActionEditorData(_actionName, _actionValue);
        }

        public static ActionEditor ImportData(ActionEditorData data)
        {
            return new ActionEditor(data.ActionName, data.ActionValue);
        }
    }

    public class ActionEditorData
    {
        [JsonProperty]
        public string ActionName = "";
        [JsonProperty]
        public string ActionValue = "";

        public ActionEditorData()
        {

        }

        public ActionEditorData(string ActionName, string ActionValue)
        {
            this.ActionName = ActionName;
            this.ActionValue = ActionValue;
        }
    }
}
