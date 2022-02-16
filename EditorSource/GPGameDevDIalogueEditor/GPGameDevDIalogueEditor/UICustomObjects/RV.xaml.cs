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

namespace GPGameDevDialogueEditor.UICustomObjects
{
    /// <summary>
    /// Logica di interazione per RV.xaml
    /// </summary>
    public partial class RV : UserControl
    {
        public RV()
        {
            InitializeComponent();
        }

        public RV(string Name)
        {
            InitializeComponent();
            CustomFunctionality.TextboxGotFocus(Tb);
            Tb.Text = Name;
            CustomFunctionality.TextboxLostFocus(Tb);
        }

        public string RVName
        {
            get
            {
                return Tb.Text;
            }
        }

        internal static int HeighValue = 20;
        public DialogueStarter parent;
        private void RemoveThis(object sender, RoutedEventArgs e)
        {
            if (Parent is StackPanel)
            {
                StackPanel ParentPanel = (StackPanel)Parent;
                ParentPanel.Children.Remove(this);
            }
            else throw new Exception("Action editor parent must be a stack panel");
            if (parent != null) parent.RectangleContainer.Height -= HeighValue;
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            window.UpdateTalkErrorOutcomes();
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
            while (CheckHomonymous())
            {
                this.Tb.Text = this.Tb.Text + "_";
            }
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            window.UpdateTalkErrorOutcomes();
        }

        private bool CheckHomonymous()
        {
            foreach (UIElement ul in parent.MainStackPanel.Children) if (ul is RV rv && rv != this && rv.Tb.Text == this.Tb.Text) return true;
            return false;
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            window.UpdateTalkErrorOutcomes();
        }

        public RVData ExportData()
        {
            return new RVData(Tb.Text);
        }

        public RuntimeVariable Export()
        {
            return new RuntimeVariable(Tb.Text);
        }

        public static RV ImportData(RVData data)
        {
            return new RV(data.Name);
        }
    }

    public class RVData
    {
        public RVData(string Name)
        {
            this.Name = Name;
        }
        public RVData()
        {

        }
        [JsonProperty]
        public string Name { get; set; } = "";
    }
}
