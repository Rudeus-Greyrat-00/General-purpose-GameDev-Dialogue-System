using System;
using System.Collections.Generic;
using System.IO;
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

namespace GPGameDevDialogueEditor
{
    internal static class CustomFunctionality
    {
        private static char ModifierStartChar = '{';
        private static char ModifierEndChar = '}';

        public static void TextboxGotFocus(TextBox textbox)
        {
            if (textbox.Tag == null || (textbox.Tag is TextWatermarkTag tag && tag.Active))
            {
                if(textbox.Tag == null) textbox.Tag = new TextWatermarkTag(false, textbox.Text);
                UnshowWatermark(textbox);
            }
            MainWindow.HasUnsavedChange = true;
        }

        public static void TextboxLostFocus(TextBox textbox)
        {
            if(textbox.Tag != null && textbox.Tag is TextWatermarkTag tag)
            {
                if (string.IsNullOrEmpty(textbox.Text))
                {
                    ShowWatermark(textbox);
                    textbox.Tag = new TextWatermarkTag(true, tag.Text);
                }
                else textbox.Tag = new TextWatermarkTag(false, tag.Text);
            }
            MainWindow.HasUnsavedChange = true;
        }

        private static void ShowWatermark(TextBox textbox)
        {
            if (textbox.Tag is TextWatermarkTag tag)
            {
                textbox.Text = tag.Text;
                textbox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private static void UnshowWatermark(TextBox textbox)
        {
            textbox.Text = "";
            textbox.Foreground = new SolidColorBrush(Colors.White);
        }

        static public string errorDesc = "";
        public static bool VerifyText(TextBox tb) //return true if it is all correct
        {
            string text = (string)tb.Text.Clone(); //text in the text box must remain untouched
            bool error = false;

           while(CheckFirstModifier(text, out error)) //while there is some other modifier in text
            {
                if (error) break;
                else
                {
                    Find_AndRemove_FirstModifier(ref text); //remove it so you can check for another modifier
                }
            }
            return !error;
        }

        private static bool CheckFirstModifier(string text, out bool error) //finds the first modifier in text. For example in this text ["Hey, your {loadRV,item} is so {loadRV,comment}"] it finds and checks {loadRV,item}
        {
            if((text.Contains(ModifierStartChar) && !text.Contains(ModifierEndChar)) || (text.Contains(ModifierEndChar) && !text.Contains(ModifierStartChar)) || text.Contains(ModifierStartChar) && text.Contains(ModifierEndChar) && text.IndexOf(ModifierEndChar) < text.IndexOf(ModifierStartChar))
            {
                errorDesc = $"Modifier declaration syntax incorrect: missing ['{ModifierEndChar}' or '{ModifierStartChar}'] after ['{ModifierStartChar}' or '{ModifierEndChar}']";
                error = true;
                return false;
            }
            else if(text.Contains(ModifierStartChar) && text.Contains(ModifierEndChar) && text.IndexOf(ModifierEndChar) > text.IndexOf(ModifierStartChar))
            {
                string mod = text.Substring(text.IndexOf(ModifierStartChar) + 1, text.IndexOf(ModifierEndChar) - text.IndexOf(ModifierStartChar) - 1);
                if(mod.Split(',').Length > 1)
                {
                    string modifierType = mod.Split(',')[0];
                    string modifierArgument = mod.Split(',')[1];
                    bool valid;
                    ParseModifier(out valid, text.IndexOf(ModifierStartChar), modifierType, modifierArgument);
                    error = !valid;
                    if (valid) return true;
                    else return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(mod) || string.IsNullOrWhiteSpace(mod)) errorDesc = "Modifier declaration incorrect: missing argument";
                    else errorDesc = $"Modifier declaration syntax incorrect for modifier [{mod}]: missing modifier's argument";
                    error = true;
                    return false;
                }
            }
            else
            {
                error = false;
                return false;
            }
        }


        private static Modifier Find_AndRemove_FirstModifier(ref string text) //find a modifier than remove it from text and return the correct modifier type. A CheckFirstModifier must be called before this
        {
            string mod = text.Substring(text.IndexOf(ModifierStartChar) + 1, text.IndexOf(ModifierEndChar) - text.IndexOf(ModifierStartChar) - 1);
            if (mod.Split(',').Length > 1)
            {
                string modifierType = mod.Split(',')[0];
                string modifierArgument = mod.Split(',')[1];
                bool valid;
                Modifier? toReturn = ParseModifier(out valid, text.IndexOf(ModifierStartChar), modifierType, modifierArgument);
                if (!valid || toReturn == null) throw new Exception("Chacking of modifiers failed");
                text = text.Remove(text.IndexOf(ModifierStartChar), mod.Length + 2); //mod lenght + '{' + '}' so --> mod lenght + 2
                return toReturn;
            }
            else
            {
                throw new NotImplementedException("Modifier must have at least 1 argument");
            }
        }


        private static Modifier? ParseModifier(out bool valid, int position, string modName, string? modValue = null)
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            bool isUnknow = true;
            switch (modName)
            {
                case "time":
                    {
                        float val;
                        if (string.IsNullOrEmpty(modValue) || !Single.TryParse(modValue, out val)) { errorDesc = $"time modifier: argument is empty or it is not a number"; isUnknow = false; goto default; }
                        else
                        {
                            val = float.Parse(modValue);
                            CharTimeEditModifier modifier = new CharTimeEditModifier(position, val);
                            valid = true;
                            return modifier;
                        }
                    }
                case "loadRV": //load runtime variable
                    {
                        if (string.IsNullOrEmpty(modValue) || string.IsNullOrWhiteSpace(modValue)) { errorDesc = $"loadRV modifier: argument is empty"; isUnknow = false; goto default; }
                        else if (!window.MainStarter.HasThisRV(modValue)) { errorDesc = $"loadRV modifier: this dialogue does not contain a runtime variable called [{modValue}], you can add runtime variables in the dialogue starter, clicking on \"Add Runtime Variable (RV)\""; isUnknow = false; goto default; }
                        else
                        {
                            LoadDialogueRuntimeVariableModifier modifier = new LoadDialogueRuntimeVariableModifier(position, modValue);
                            valid = true;
                            return modifier;
                        }
                    }
                default:
                    {
                        if (isUnknow) errorDesc = $"Unknow modifier [{modName}], correct modifier declaration example: {{loadRV,argument}}";
                        valid = false;
                        return null;
                    }
            }
        }

        public static List<Modifier> ExportModifier(string textIN, out string dialogueText)
        {
            List<Modifier> mods = new List<Modifier>();
            string text = new string(textIN);
            bool error = false;
            while (CheckFirstModifier(text, out error)) //while there is some other modifier in text
            {

                if (!error) mods.Add(Find_AndRemove_FirstModifier(ref text)); //remove it so you can check for another modifier
                else { MessageBox.Show("Some modifier cannot be exported due to errors in modifier declarations. Please check the small rectangle in the upper-right corner of every talk's text textbox. If it is red, hover that with the mouse to obtain more information, or check the documentation"); break; }
            }
            dialogueText = text;
            return mods;
        }

        public static string ExportDataToJsonString()
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            if (window.MainStarter.starter.NextTalk != null) window.MainStarter.starter.NextTalk.IS_DIALOGUE_STARTER = true;
            //else throw new Exception("NO DIALOGUE LINKED TO THE STARTER");
            List<DialogueObjectEditors.TalkEditor> talkEditors = new List<DialogueObjectEditors.TalkEditor>();
            foreach(UIElement el in window.MainCanvas.Children) if(el is DialogueObjectEditors.TalkEditor tk) talkEditors.Add(tk);

            EDPData data = new EDPData(talkEditors, window.MainStarter.ExportData());

            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.TypeNameHandling = TypeNameHandling.All;

            TextWriter textWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, data);

            return textWriter.ToString();
        }

        public static EDPData ImportDataFromJsonString(string value)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            serializer.TypeNameHandling = TypeNameHandling.All;

            TextReader reader = new StringReader(value);
            JsonReader JSONreader = new JsonTextReader(reader);
            EDPData? data = null;

            try { data = (EDPData)serializer.Deserialize(JSONreader); }
            catch { MessageBox.Show("The EDP file is corrupted or incompatible with this version of the program, so it cannot be opened.", "EDP loader error", MessageBoxButton.OK, MessageBoxImage.Error); return null; }
            CurrentEdpData = data; //when an EDP project is loaded, currentedpdata is setted. It will be uesd almost immediatly after to spawn all the talks
            if (data != null) return data;
            else throw new Exception("JSON deserializer returned null");
        }

        public static void SpawnTalkInCanvas(Int32 hash) //from current edp data
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            if (CurrentEdpData != null)
            {
                if (EdpContainTalkWithHash(CurrentEdpData, hash))
                {
                    foreach (DialogueObjectEditors.TalkEditorData tkdata in CurrentEdpData.talks) if (tkdata.HashValue == hash) DialogueObjectEditors.TalkEditor.ImportData(tkdata); //it adds itself to the main canvas, see TalkEditor.ImportData

                    //throw new Exception("bug somewhere around line 235");
                }
                else throw new Exception($"Current edp does not contain a talk with hash [{hash}]");
            }
            else throw new NullReferenceException("Current Edp Data variable is null");
        }

        private static bool EdpContainTalkWithHash(EDPData mainData, Int32 hash)
        {
            foreach(DialogueObjectEditors.TalkEditorData data in mainData.talks) if(data.HashValue == hash) return true;

            return false;
        }

        public static bool LoadAllTalksFromEDP()
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            if (CurrentEdpData != null)
            {
                foreach (DialogueObjectEditors.TalkEditorData tkdata in CurrentEdpData.talks) if (DialogueObjectEditors.TalkEditor.ExistInCanvas(tkdata.HashValue) == false) SpawnTalkInCanvas(tkdata.HashValue);
                window.MainStarter.ImportData(CurrentEdpData.starterData);
                foreach (UIElement el in window.MainCanvas.Children) if (el is DialogueObjectEditors.TalkEditor tkedit) 
                    {
                        if (tkedit.IS_DIALOGUE_STARTER) { window.MainStarter.starter.NextTalk = tkedit; break; }
                    }
                window.UpdateTalkErrorOutcomes();
                return true;
            }
            else return false;
        }

        public static EDPData ExportFromCanvas()
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            EDPData toReturn = new EDPData();
            if (window.MainStarter.starter.NextTalk != null) window.MainStarter.starter.NextTalk.IS_DIALOGUE_STARTER = true;
            foreach (UIElement el in window.MainCanvas.Children) if (el is DialogueObjectEditors.TalkEditor tk) toReturn.talks.Add(tk.ExportData());
            toReturn.starterData = window.MainStarter.ExportData();
            return toReturn;
        }

        public static EDPData? CurrentEdpData { get; set; }
    }

    public class EDPData
    {
        [JsonProperty]
        public List<DialogueObjectEditors.TalkEditorData> talks = new List<DialogueObjectEditors.TalkEditorData>();
        [JsonProperty]
        public UICustomObjects.DialogueStarterData? starterData = null;
        public EDPData()
        {

        }

        public EDPData(List<DialogueObjectEditors.TalkEditor> TalkEditors, UICustomObjects.DialogueStarterData starterData)
        {
            foreach (DialogueObjectEditors.TalkEditor tk in TalkEditors) talks.Add(tk.ExportData());
            this.starterData = starterData;
        }
    }

    public struct TextWatermarkTag
    {
        public TextWatermarkTag(bool active, string text)
        {
            this.Active = active;
            this.Text = text;
        }

        public bool Active = true; // = is showing the watermark
        public string Text = "";
    }
}
