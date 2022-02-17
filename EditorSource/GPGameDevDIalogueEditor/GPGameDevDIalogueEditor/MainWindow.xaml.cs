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
using System.Threading;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Windows.Markup;

namespace GPGameDevDialogueEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainScrollViewer.ScrollToHorizontalOffset(10000);
            MainScrollViewer.ScrollToVerticalOffset(10000);
            MainScrollViewer.UpdateLayout();
            defaultTransform = MainCanvas.RenderTransform.Clone();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                UpdateLines();
            }).Start();

        }

        //UI:
        UIElement? dragObject = null;
        public DialogueObjectEditors.TalkEditor? dropTarget = null;
        Point offset;

        //backend stuff:
        delegate void UpdateLinesBackgroundCallback();

        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;

        Transform defaultTransform;

        public void Control_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragObject = sender as UIElement;
            foreach (UIElement el in MainCanvas.Children) if (!(el is UICustomObjects.LinkLine)) Canvas.SetZIndex(el, 1);
            if (dragObject != null && dragObject != this)
            {
                Canvas.SetZIndex(dragObject, 2);
                offset = e.GetPosition(MainCanvas); //current position of the mouse in canvas
                offset.Y -= Canvas.GetTop(dragObject); //current position of the object in canvas
                offset.X -= Canvas.GetLeft(dragObject); //without subtracting this when click the object it will always go to the top left corner of the canvas
                MainCanvas.CaptureMouse();
            }
        }

        private void MainCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (dragObject != null)
            {
                var position = e.GetPosition(sender as UIElement); //position of the mouse in the canvas
                                                                   //without subtracting this when click the object it will be taken from the upper left corner of it
                MoveSpecific(position.X - offset.X, position.Y - offset.Y);
            }
        }

        private void MainCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.ReleaseMouseCapture();
            OnMouseUpOnDragObject();
            dragObject = null;

        }

        private void AddTalk(object sender, RoutedEventArgs e)
        {
            DialogueObjectEditors.TalkEditor newTalkEditor = new DialogueObjectEditors.TalkEditor();
            MainCanvas.Children.Add(newTalkEditor);
            foreach (UIElement el in MainCanvas.Children) if (!(el is UICustomObjects.LinkLine)) Canvas.SetZIndex(el, 1);
            Canvas.SetTop(newTalkEditor, 3);
            Canvas.SetLeft(newTalkEditor, Mouse.GetPosition(MainCanvas).X + 10);
            Canvas.SetTop(newTalkEditor, Mouse.GetPosition(MainCanvas).Y + 10);
        }

        private void MoveSpecific(double newX, double newY)
        {
            if (dragObject is UICustomObjects.LinkLine ll)
            {
                //remove this part
            }
            else if (dragObject is DialogueObjectEditors.TalkEditor t)
            {
                Canvas.SetTop(dragObject, newY);
                Canvas.SetLeft(dragObject, newX);
            }
        }

        private void UpdateLinesMethod()
        {
            for (int i = 0; i < MainCanvas.Children.Count; i++)
            {
                if (MainCanvas.Children[i] is UICustomObjects.LinkLine ll) ll.Update();
            }
        }

        private void UpdateLines()
        {
            while (true)
            {
                Dispatcher.BeginInvoke(new UpdateLinesBackgroundCallback(UpdateLinesMethod));
                Thread.Sleep(10);
            }
        }

        private void OnMouseUpOnDragObject()
        {
            
            //throw new NotImplementedException($"drop target null == {dropTarget == null}");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        float currDelta = 0;
        float maxDelta = 5000;
        private void Zoom(object sender, MouseWheelEventArgs e)
        {
            var matTrans = MainCanvas.RenderTransform as MatrixTransform;
            var zoom_offset = e.GetPosition(MainScrollViewer);

            currDelta += e.Delta;

            if(Math.Abs(currDelta) > maxDelta)
            {
                if (currDelta > 0) currDelta = maxDelta;
                if (currDelta < 0) currDelta = -maxDelta;
            }

            if(Math.Abs(currDelta) < maxDelta)
            {
                var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;

                var mat = matTrans.Matrix;
                mat.ScaleAt(scale, scale, MainScrollViewer.HorizontalOffset + zoom_offset.X, MainScrollViewer.VerticalOffset + zoom_offset.Y);
                matTrans.Matrix = mat;
            }

            e.Handled = true;
        }

        private void MainScroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                
                scrollMousePoint = e.GetPosition(MainScrollViewer);
                hOff = MainScrollViewer.HorizontalOffset;
                vOff = MainScrollViewer.VerticalOffset;
                MainScrollViewer.CaptureMouse();
            }
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (MainScrollViewer.IsMouseCaptured)
            {
                MainScrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(MainScrollViewer).X));
                MainScrollViewer.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(MainScrollViewer).Y));
            }
        }

        private void scrollViewer_PreviewMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainScrollViewer.ReleaseMouseCapture();
        }

        private void ResetView(object sender, RoutedEventArgs e)
        {
            MainScrollViewer.ScrollToHorizontalOffset(10000);
            MainScrollViewer.ScrollToVerticalOffset(10000);
            MainScrollViewer.UpdateLayout();
            MainCanvas.RenderTransform = defaultTransform.Clone();
            currDelta = 0;
        }

        public void UpdateTalkErrorOutcomes()
        {
            foreach (UIElement el in MainCanvas.Children) if (el is DialogueObjectEditors.TalkEditor tk) tk.UpdateErrorOutcome();
        }

        string? currentSavePath = null;
        public static bool HasUnsavedChange = false;
        private bool SaveEsit = false;

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = ""; // Default file name
            dialog.DefaultExt = ".EDP"; // Default file extension
            dialog.Filter = "Editor Dialogue Project (editor only) (.EDP)|*.EDP"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            //EDPData toExport = CustomFunctionality.ExportFromCanvas();
            string exportedProjectToString = CustomFunctionality.ExportDataToJsonString();

            // Process save file dialog box results
            if (result == true)
            {

                string filename = dialog.FileName;



                File.WriteAllText(filename, exportedProjectToString);
                currentSavePath = filename;
                HasUnsavedChange = false;
            }
            if (result == null) SaveEsit = false; 
            else SaveEsit = (bool)result;
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Exported_Dialogue"; // Default file name
            dialog.DefaultExt = ".json"; // Default file extension
            dialog.Filter = "Dialogue json file (not editor editable) (.json)|*.json"; // Filter files by extension

            foreach (UIElement el in MainCanvas.Children) if (el is DialogueObjectEditors.TalkEditor tk) tk.Exported = false; //reset all the "exported" status of current objects

            bool? result = dialog.ShowDialog();

            Dialogue exportedDialogue;
            Talk? firstTalk = null;
            if (MainStarter.starter.NextTalk != null) firstTalk = MainStarter.starter.NextTalk.Export();
            if (firstTalk == null) exportedDialogue = new Dialogue(MainStarter.dialogName.Text, MainStarter.dialogLanguage.Text);
            else exportedDialogue = new Dialogue(MainStarter.dialogName.Text, MainStarter.dialogLanguage.Text, firstTalk);
            exportedDialogue.runtimeVariables = MainStarter.ExportRV();
            string exportedDialogueToString = DialogueSystem.DialogueUtilities.ConvertToJSONString(exportedDialogue);

            // Process save file dialog box results
            if (result == true)
            {

                string filename = dialog.FileName;



                File.WriteAllText(filename, exportedDialogueToString);


            }
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".EDP"; // Default file extension
            dialog.Filter = "Editor Dialogue Project (editor only) (.EDP)|*.EDP"; // Filter files by extension

            if (HasUnsavedChange) 
            { 
                MessageBoxResult messageBoxResult = MessageBox.Show("There are unsaved changes to the current project! Would you like to save before opening another file?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning); 
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        {
                            Save(sender, e);
                            if (SaveEsit) { HasUnsavedChange = false; break; }
                            else return;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        {
                            return;
                        }
                }
            }


            if(MainCanvas.Children.Count > 1) MainCanvas.Children.RemoveRange(1, MainCanvas.Children.Count);
            if (MainCanvas.Children[0] != null && MainCanvas.Children[0] is UICustomObjects.DialogueStarter starter) starter.ClearData();
           
            bool? result = dialog.ShowDialog();

            if (result == true)
            {

                string filename = dialog.FileName;



                string toLoad = File.ReadAllText(filename);
                CustomFunctionality.ImportDataFromJsonString(toLoad);
                if (CustomFunctionality.LoadAllTalksFromEDP())
                {
                    currentSavePath = filename;
                    HasUnsavedChange = false;
                }
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if(currentSavePath != null)
            {
                string exportedProjectToString = CustomFunctionality.ExportDataToJsonString();
                File.WriteAllText(currentSavePath, exportedProjectToString);
                HasUnsavedChange = false;
                SaveEsit = true;
            }
            else
            {
                SaveAs(sender, e);
            }
        }

        private void New(object sender, RoutedEventArgs e)
        {
            if (HasUnsavedChange)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("There are unsaved changes to the current project! Would you like to save before starting a new one?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        {
                            Save(sender, e);
                            if (SaveEsit) { HasUnsavedChange = false; break; }
                            else return;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        {
                            return;
                        }
                }
            }

            if (MainCanvas.Children.Count > 1) MainCanvas.Children.RemoveRange(1, MainCanvas.Children.Count);
            if (MainCanvas.Children[0] != null && MainCanvas.Children[0] is UICustomObjects.DialogueStarter starter) starter.ClearData();
            HasUnsavedChange = false;
        }

        private void ShortcoutManager(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.S:
                        {
                            Save(sender, new RoutedEventArgs());
                            return;
                        }
                    case Key.E:
                        {
                            Export(sender, new RoutedEventArgs());
                            return;
                        }
                    case Key.T:
                        {
                            AddTalk(sender, new RoutedEventArgs());
                            return;
                        }
                }
            }
            if (Keyboard.IsKeyDown(Key.F5))
            {
                TestDialogue(sender, new RoutedEventArgs());
            }
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                DialogueSimulationUIElement.Visibility = Visibility.Hidden;
            }
        }

        private void TestDialogue(object sender, RoutedEventArgs e)
        {
            DialogueSimulationUIElement.Visibility = Visibility.Visible;

            foreach (UIElement el in MainCanvas.Children) if (el is DialogueObjectEditors.TalkEditor tk) tk.Exported = false; //reset all the "exported" status of current objects

            Dialogue testDialogue;
            Talk? firstTalk = null;
            if (MainStarter.starter.NextTalk != null) firstTalk = MainStarter.starter.NextTalk.Export();
            if (firstTalk == null) testDialogue = new Dialogue(MainStarter.dialogName.Text, MainStarter.dialogLanguage.Text);
            else testDialogue = new Dialogue(MainStarter.dialogName.Text, MainStarter.dialogLanguage.Text, firstTalk);
            testDialogue.runtimeVariables = MainStarter.ExportRV();

            DialogueSimulationUIElement.SimulateDialogue(testDialogue);

        }

        private void ShowDialogueSimulation(object sender, RoutedEventArgs e)
        {
            DialogueSimulationUIElement.Visibility = Visibility.Visible;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            if (HasUnsavedChange)
            {
                MessageBoxResult result = MessageBox.Show("Quit without saving?", "Quitting", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) Close();
            }
            else Close();
        }
    }
}
