using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Logica di interazione per TalkEditor.xaml
    /// </summary>
    public partial class TalkEditor : UserControl
    {
        public TalkEditor()
        {
            InitializeComponent();
            MainWindow.HasUnsavedChange = true;
        }

        public TalkEditor(string TalkName, string? CharacterName, string TalkText)
        {
            InitializeComponent();
            if (CharacterName == null) CharacterName = "";
            CustomFunctionality.TextboxGotFocus(TalkTag);
            CustomFunctionality.TextboxGotFocus(this.CharacterName);
            CustomFunctionality.TextboxGotFocus(MainTextBox);
            TalkTag.Text = TalkName;
            this.CharacterName.Text = CharacterName;
            MainTextBox.Text = TalkText;
            CustomFunctionality.TextboxLostFocus(TalkTag); 
            CustomFunctionality.TextboxLostFocus(this.CharacterName);
            CustomFunctionality.TextboxLostFocus(MainTextBox);
        }
        public bool isStarterTalk = false;
        public bool IS_DIALOGUE_STARTER
        {
            get
            {
                return isStarterTalk;
            }
            set
            {
                GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
                foreach (UIElement el in window.MainCanvas.Children) if (el is TalkEditor tk) tk.isStarterTalk = false;
                isStarterTalk = value;  
            }
        }

        private void AddAnswer(object sender, RoutedEventArgs e)
        {
            AnswerEditor answerEditor = new AnswerEditor();
            answerEditor.container = this;
            AnswerStackPanel.Children.Add(answerEditor);
            RectangleContainer.Height += AnswerEditor.HeighValue;
            UpdateLineValidation();
        }

        private void AddCustomAction(object sender, RoutedEventArgs e)
        {
            ActionEditor actionEditor = new ActionEditor();
            actionEditor.talkContainer = this;
            ActionStackPanel.Children.Add(actionEditor);
            RectangleContainer.Height += ActionEditor.HeighValue;
        }

        private void AddCondition(object sender, RoutedEventArgs e) //it should never be more than one condition
        {
            ConditionEditor condition = new ConditionEditor();
            condition.container = this;
            RectangleContainer.Height += ConditionEditor.HeighValue;
            ConditionContainer.Children.Add(condition);
            UpdateLineValidation();
        }

        private void RemoveCondition(object sender, RoutedEventArgs e) //it should never be more than one condition
        {
            int counter = 0; foreach (object obj in ConditionContainer.Children) if (obj is ConditionEditor c) { counter += c.ActionCount; c.falseStarter.DeleteLine(); c.trueStarter.DeleteLine(); }
            ConditionContainer.Children.Clear();
            RectangleContainer.Height -= (ConditionEditor.HeighValue + counter * ActionEditor.HeighValue);
            UpdateLineValidation();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            window.Control_PreviewMouseDown(this, e);
        }

        public void UpdateLineValidation()
        {
            if (ThereAreAnswers() || ThereIsACondition()) 
            { 
                starter.InvalidLine(); 
                if(ThereIsACondition() && ThereAreAnswers())
                {
                    ConditionEditor c = getCurrentCondition();
                    c.trueStarter.InvalidLine(); c.falseStarter.InvalidLine();
                }
                else if(ThereIsACondition() && !ThereAreAnswers())
                {
                    ConditionEditor c = getCurrentCondition();
                    c.trueStarter.ValidLine(); c.falseStarter.ValidLine();
                }
            }
            else starter.ValidLine();
        }

        private bool ThereAreAnswers()
        {
            return AnswerStackPanel.Children.Count > 0;
        }

        private bool ThereIsACondition()
        {
            if (ConditionContainer.Children.Count > 0 && ConditionContainer.Children[0] != null && ConditionContainer.Children[0] is ConditionEditor) return true;
            else return false;
        }

        private ConditionEditor? getCurrentCondition()
        {
            if (ConditionContainer.Children.Count > 0 && ConditionContainer.Children[0] != null && ConditionContainer.Children[0] is ConditionEditor c)
            {
                return c;
            }
            else return null;
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                CustomFunctionality.TextboxGotFocus( textbox);
            }
            else throw new Exception("This function must be called only by text box");
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                CustomFunctionality.TextboxLostFocus( textbox);
            }
            else throw new Exception("This function must be called only by text box");
        }

        private void RemoveThis(object sender, RoutedEventArgs e)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            window.MainCanvas.Children.Remove(this);
            foreach(UICustomObjects.LinkLine line in Lines) if(line != null && window.MainCanvas.Children.Contains(line)) window.MainCanvas.Children.Remove(line);
        }

        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            UICustomObjects.LinkLine linkLine = (UICustomObjects.LinkLine)e.Data.GetData(typeof(UICustomObjects.LinkLine));
            linkLine.dropped = true;
            linkLine.EndTalk = this;
            window.MainCanvas.Children.Add(linkLine);
            Canvas.SetZIndex(linkLine,10);
            linkLine.starter.lineStarter.AttachedLinkLine = linkLine; //confirm to the linkline starter this linkline is a good one, so the starter will store this linkline object
            if (linkLine.StartTalk == null) this.IS_DIALOGUE_STARTER = true;
        }

        private List<UICustomObjects.LinkLine> Lines
        {
            get
            {
                List<UICustomObjects.LinkLine> toReturn = new List<UICustomObjects.LinkLine>();
                GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
                for(int i = 0; i < window.MainCanvas.Children.Count; i++) if(window.MainCanvas.Children[i] is UICustomObjects.LinkLine ll) if(ll.EndTalk == this || ll.StartTalk == this) toReturn.Add(ll);
                return toReturn;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                UpdateErrorOutcome();
            }
        }

        public void UpdateErrorOutcome()
        {
            if (CustomFunctionality.VerifyText(MainTextBox)) { parseModifierOutcome.Fill = new SolidColorBrush(Colors.Green); parseModifierOutcome.ToolTip = "No errors found"; }
            else { parseModifierOutcome.Fill = new SolidColorBrush(Colors.Red); parseModifierOutcome.ToolTip = CustomFunctionality.errorDesc; }
        }

        public bool Exported = false;
        private Talk ExportedTalk;
        public Talk Export()
        {
            if(Exported) return ExportedTalk;
            else
            {
                List<DialogueAction> actions = new List<DialogueAction>();
                List<Answer> answers = new List<Answer>();

                string? character = CharacterName.Text == "Name of the talking character" ? null : CharacterName.Text;
                string ttag = TalkTag.Text != "For example 'Statement'" ? TalkTag.Text : "";
                string ttext = MainTextBox.Text != "What the character says..." ? MainTextBox.Text : "";
                string talkText;
                List<Modifier> modifiers = new List<Modifier>();
                modifiers = CustomFunctionality.ExportModifier(ttext, out talkText);
                Talk talk = new Talk(talkText, ttag, character);

                ExportedTalk = talk;
                Exported = true;

                DialogueSystem.Condition? condition;
                foreach (UIElement el in ActionStackPanel.Children) if (el is ActionEditor ae) actions.Add(ae.Export());
                foreach (UIElement el in AnswerStackPanel.Children) if (el is AnswerEditor answ) answers.Add(answ.Export());
                if (ConditionContainer.Children.Count > 0 && ConditionContainer.Children[0] != null && ConditionContainer.Children[0] is ConditionEditor ce) condition = ce.Export();
                else condition = null;

                talk.answers = answers;
                talk.modifiers = modifiers;
                talk.TalkCondition = condition;
                talk.unconditionalActions = actions;
                if(starter.NextTalk != null) talk.UnconditionalNext = starter.NextTalk.Export();

                return talk;
            }
        }

        public Int32 Hash()
        {
            string source = TalkTag.Text + MainTextBox.Text + CharacterName.Text + Canvas.GetLeft(this) + Canvas.GetTop(this) + Canvas.GetZIndex(this);
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF32.GetBytes(source));
            var ivalue = BitConverter.ToInt32(hashed, 0);
            return ivalue;
        }

        public TalkEditorData ExportData()
        {
            List<ActionEditorData> actions = new List<ActionEditorData>();
            List<AnswerEditorData> answers = new List<AnswerEditorData>();
            foreach (UIElement el in ActionStackPanel.Children) if (el is ActionEditor ae) actions.Add(ae.ExportData()); 
            foreach (UIElement el in AnswerStackPanel.Children) if (el is AnswerEditor answ) answers.Add(answ.ExportData());
            ConditionEditorData? condition = null;
            if(ConditionContainer.Children.Count > 0 && ConditionContainer.Children[0] != null && ConditionContainer.Children[0] is ConditionEditor ed) condition = ed.ExportData();
            Int32? next = null;
            if (starter.NextTalk != null) next = starter.NextTalk.Hash();
            string ttag = TalkTag.Text != "For example 'Statement'" ? TalkTag.Text : "";
            string ttext = MainTextBox.Text != "What the character says..." ? MainTextBox.Text : "";
            string tcharactername = CharacterName.Text != "Name of the talking character" ? CharacterName.Text : "";
            return new TalkEditorData(this.Hash(), ttag, tcharactername, ttext, next, Canvas.GetTop(this), Canvas.GetLeft(this), Canvas.GetZIndex(this), condition, actions, answers, (bool)IsConditionalCB.IsChecked, IS_DIALOGUE_STARTER);
        }

        public static TalkEditor ImportData(TalkEditorData data)
        {
            //throw new NotImplementedException();
            //TODO modificare gli importdata in modo che funzionino come questo, quindi si aggiungono da soli alla talk (che si passa come altro argomento)
            TalkEditor talk = new TalkEditor(data.TalkName, data.CharacterName, data.Text);
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            talk.isStarterTalk = data.IsSTARTER;
            window.MainCanvas.Children.Add(talk);
            Canvas.SetLeft(talk, data.CanvasLeft); Canvas.SetTop(talk, data.CanvasTop); Canvas.SetZIndex(talk, data.CanvasZIndex); //positions the talk editor in the canvas

            foreach (ActionEditorData ac in data.actions) 
            {
                ActionEditor actionEditor = ActionEditor.ImportData(ac);
                actionEditor.talkContainer = talk;
                talk.RectangleContainer.Height += ActionEditor.HeighValue;
                talk.ActionStackPanel.Children.Add(actionEditor); 
            }
            foreach (AnswerEditorData aw in data.answers) 
            {
                AnswerEditor answerEditor = AnswerEditor.ImportData(aw, talk);
                talk.RectangleContainer.Height += answerEditor.CalculateHeightValue();
             
                talk.AnswerStackPanel.Children.Add(answerEditor);

            }

            talk.IsConditionalCB.IsChecked = data.IsConditional;
            talk.ConditionContainer.Children.Clear();
            if (data.IsConditional) talk.ConditionContainer.Children.Add(ConditionEditor.ImportData(data.condition, talk)); //add eventual condition

            if (data.next != null && TalkEditor.ExistInCanvas((Int32)data.next)) talk.starter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.next); 
            else if (data.next != null) { CustomFunctionality.SpawnTalkInCanvas((Int32)data.next); talk.starter.NextTalk = TalkEditor.GetFromCanvas((Int32)data.next); }

            talk.UpdateLineValidation();

            return talk;
        }

        public static bool ExistInCanvas(Int32 hash)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            foreach (UIElement el in window.MainCanvas.Children) if(el is TalkEditor tk && tk.Hash() == hash) { return true; }
            return false;
        }

        public static TalkEditor GetFromCanvas(Int32 hash)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            if (ExistInCanvas(hash))
            {
                foreach (UIElement el in window.MainCanvas.Children) if (el is TalkEditor tk && tk.Hash() == hash) { return tk; }
                //else if not found
                throw new NullReferenceException($"Talk with hash = [{hash}] does not exists in main canvas");
            }
            else throw new NullReferenceException($"Talk with hash = [{hash}] does not exists in main canvas");
        }
    }

    public class TalkEditorData
    {
        [JsonProperty]
        public string TalkName = "";
        [JsonProperty]
        public string CharacterName = "";
        [JsonProperty]
        public string Text = "";
        [JsonProperty]
        public List<AnswerEditorData> answers = new List<AnswerEditorData>();
        [JsonProperty]
        public List<ActionEditorData> actions = new List<ActionEditorData>();
        [JsonProperty]
        public ConditionEditorData? condition;
        [JsonProperty]
        public Int32? next;
        [JsonProperty]
        public double CanvasTop = -1;
        [JsonProperty]
        public double CanvasLeft = -1;
        [JsonProperty]
        public int CanvasZIndex = -1;
        [JsonProperty]
        public Int32 HashValue;
        [JsonProperty]
        public bool IsSTARTER = false;
        [JsonProperty]
        public bool IsConditional = false;

        public TalkEditorData()
        {

        }

        public TalkEditorData(Int32 hashValue, string TalkName, string CharacterName, string TalkText, Int32? next, double CanvasTop, double CanvasLeft, int CanvasZIndex, ConditionEditorData? condition,  List<ActionEditorData>? INactions = null, List<AnswerEditorData>? INanswers = null, bool IsConditional = false, bool IsSTARTER = false)
        {
            this.TalkName = TalkName; this.CharacterName = CharacterName; this.Text = TalkText;
            this.next = next; this.CanvasTop = CanvasTop; this.CanvasLeft = CanvasLeft; this.CanvasZIndex = CanvasZIndex;
            this.condition = condition;
            if(INactions != null) actions = INactions;
            if(INanswers != null) answers = INanswers;
            this.IsConditional = IsConditional;
            this.IsSTARTER = IsSTARTER;
            this.HashValue = hashValue;
        }
    }
}
