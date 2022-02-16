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
using System.Diagnostics;

namespace GPGameDevDialogueEditor.UICustomObjects
{
    /// <summary>
    /// Logica di interazione per LineStarter.xaml
    /// </summary>
    public partial class LineStarter : UserControl
    {
        LinkLine? linkLine = null;
        public bool IsValid = true;

        public void DeleteLine()
        {
            if(linkLine != null) linkLine.DeleteLine();
            linkLine = null;
        }

        public void ValidLine()
        {
            if(linkLine != null)
            {
                linkLine.Line.Fill = new SolidColorBrush(Colors.GhostWhite);
                linkLine.Line.Stroke = new SolidColorBrush(Colors.GhostWhite);
                linkLine.Line.StrokeDashArray = null;
            }
            x1.Visibility = Visibility.Hidden;
            x2.Visibility = Visibility.Hidden;
            ToolTipRectangle.Visibility = Visibility.Hidden;
            IsValid = true;
        }

        public void InvalidLine()
        {
            if(linkLine != null)
            {
                linkLine.Line.Stroke = new SolidColorBrush(Colors.Red);
                linkLine.Line.Fill = new SolidColorBrush(Colors.Red);
                List<double> array = new List<double> { 2, 4 };
                linkLine.Line.StrokeDashArray = new System.Windows.Media.DoubleCollection(array);
            }
            x1.Visibility = Visibility.Visible;
            x2.Visibility = Visibility.Visible;
            ToolTipRectangle.Visibility = Visibility.Visible;
            IsValid = false;
        }

        public LinkLine? AttachedLinkLine
        {
            get
            {
                return linkLine;
            }
            set
            {
                if(linkLine != null && value != null) linkLine.DeleteLine();
                linkLine = value;
            }
        }

        public LineStarter()
        {
            InitializeComponent();
        }

        public DialogueObjectEditors.TalkEditor? ParentTalk
        {
            get
            {
                if (FindParent() != null && FindParent() is DialogueObjectEditors.TalkEditor tk)
                    return tk;
                else return null;
            }

        }

        public DialogueObjectEditors.TalkEditor? NextTalk
        {
            get
            {
                if (linkLine != null && linkLine.EndTalk != null) return linkLine.EndTalk;
                else return null;
            }
            set
            {
                if(value != null)
                {
                    if (linkLine != null) linkLine.DeleteLine();
                    linkLine = new LinkLine(this, ParentTalk != null);
                    linkLine.EndTalk = value;
                    GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
                    window.MainCanvas.Children.Add(linkLine);
                    Canvas.SetZIndex(linkLine,10);
                    if (ParentTalk != null)linkLine.StartTalk = ParentTalk;
                    Debug.WriteLine(ParentTalk == null);
                }
            }
        }

        private object? FindParent()
        {
            object obj = this;
            while ((obj is UserControl || obj is FrameworkElement) && !(obj is DialogueObjectEditors.TalkEditor) && !(obj is DialogueStarter))
            {
                if(obj is DialogueObjectEditors.ConditionEditor conditionEditor)
                {
                    return conditionEditor.container;
                }
                else if(obj is DialogueObjectEditors.AnswerEditor answerEditor)
                {
                    return answerEditor.container;
                }
                else
                {
                    if (obj is UserControl uc) obj = uc.Parent;
                    else if (obj is FrameworkElement fl) obj = fl.Parent;
                }
            }
            if (obj is DialogueObjectEditors.TalkEditor tk) return tk;
            else if (obj is DialogueStarter ds) return ds;
            else return null;
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse el && FindParent() != null && FindParent() is DialogueObjectEditors.TalkEditor tk && tk.Parent is Canvas c && IsValid)
            {
                GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
                LinkLine line = new LinkLine(this);
                line.StartTalk = ParentTalk;

                DragDrop.DoDragDrop(line, line, DragDropEffects.Link);
            }
            else if(FindParent() != null && FindParent() is DialogueStarter ds)
            {
                GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
                LinkLine line = new LinkLine(this, false);
                line.StartTalk = null;

                DragDrop.DoDragDrop(line, line, DragDropEffects.Link);
            }
        }
    }
}
