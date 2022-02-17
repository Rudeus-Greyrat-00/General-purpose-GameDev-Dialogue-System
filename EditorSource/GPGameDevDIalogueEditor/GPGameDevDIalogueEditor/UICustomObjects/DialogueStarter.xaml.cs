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
    /// Logica di interazione per DialogueStarter.xaml
    /// </summary>
    public partial class DialogueStarter : UserControl
    {
        public DialogueStarter()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RV newRV = new RV();
            newRV.parent = this;
            MainStackPanel.Children.Add(newRV);
            MainStackPanel.UpdateLayout();
            RectangleContainer.Height += RV.HeighValue;
        }

        public bool HasThisRV(string rvName)
        {
            foreach(UIElement ui in MainStackPanel.Children) if(ui is RV rv) if(rv.RVName == rvName) return true;
            return false;
        }

        public void ClearData()
        {
            dialogLanguage.Text = "";
            dialogName.Text = "";
            RectangleContainer.Height -= MainStackPanel.Children.Count * RV.HeighValue;
            starter.NextTalk = null;
            starter.AttachedLinkLine = null;
            MainStackPanel.Children.Clear();
        }

        public DialogueStarterData ExportData()
        {
            List<RVData> data = new List<RVData>();
            foreach (UIElement el in MainStackPanel.Children) if (el is RV rv) data.Add(rv.ExportData());
            return new DialogueStarterData(dialogName.Text, dialogLanguage.Text, data);
        }

        public void ImportData(DialogueStarterData data)
        {
            this.dialogLanguage.Text = data.Language;
            this.dialogName.Text = data.DialogueName;
            foreach (RVData rvdat in data.rVs) 
            { 
                RV toAdd = RV.ImportData(rvdat);
                toAdd.parent = this;
                MainStackPanel.Children.Add(toAdd);
                this.RectangleContainer.Height += RV.HeighValue; 
            }
        }

        public List<RuntimeVariable> ExportRV()
        {
            List<RuntimeVariable> result = new List<RuntimeVariable>();
            foreach (UIElement el in MainStackPanel.Children) if (el is RV rv) result.Add(rv.Export());
            return result;
        }
    }

    public class DialogueStarterData
    {
        [JsonProperty]
        public string Language = "";
        [JsonProperty]
        public string DialogueName = "";
        [JsonProperty]
        public List<RVData> rVs = new List<RVData>();

        public DialogueStarterData()
        {

        }

        public DialogueStarterData(string name, string language, List<RVData> rVs)
        {
            this.DialogueName = name;
            this.Language = language;
            this.rVs = rVs;
        }
    }
}
