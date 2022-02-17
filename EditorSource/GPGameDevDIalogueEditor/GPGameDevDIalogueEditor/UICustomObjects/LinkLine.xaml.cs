using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GPGameDevDialogueEditor.UICustomObjects
{
    /// <summary>
    /// Logica di interazione per LinkLine.xaml
    /// </summary>
    public partial class LinkLine : UserControl
    {
        public bool dropped = false;
        private bool checkStart = true;
        public LinkLine(LineStarter starter)
        {
            InitializeComponent();
            this.starter = starter;
        }

        public LinkLine(LineStarter starter, bool checkStart)
        {
            InitializeComponent();
            this.starter = starter;
            this.checkStart = checkStart;
        }
        public Point Start
        {
            get
            {
                return new Point(Line.X1, Line.Y1);
            }
            set
            {
                Line.X1 = value.X;
                Line.Y1 = value.Y;
            }
        }

        public DialogueObjectEditors.TalkEditor? StartTalk { get; set; }
        public DialogueObjectEditors.TalkEditor? EndTalk { get; set; }

        public LineStarter starter { get; private set; }

        public Point End
        {
            get
            {
                return new Point(Line.X2, Line.Y2);
            }
            set
            {
                Line.X2 = value.X;
                Line.Y2 = value.Y;
            }
        }

        public void Update()
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            if (starter != null && (StartTalk != null || checkStart == false) && EndTalk != null && window != null)
            {
                Point locationFromWindow = starter.TranslatePoint(new Point(0, 0), window.MainCanvas);

                Point newLocation = new Point(locationFromWindow.X + 7, locationFromWindow.Y + 7);

                this.Start = newLocation;

                locationFromWindow = EndTalk.TranslatePoint(new Point(0, 0), window.MainCanvas);

                newLocation = new Point(locationFromWindow.X + 7, locationFromWindow.Y + 7);

                this.End = newLocation;
            }
            else if(!dropped && starter != null && (StartTalk != null || checkStart == false) && window != null)
            {
                Point locationFromWindow = starter.TranslatePoint(new Point(0, 0), window.MainCanvas);

                Point newLocation = new Point(locationFromWindow.X + 7, locationFromWindow.Y + 7);

                if (locationFromWindow == new Point(0, 0)) DeleteLine();
                this.Start = newLocation;

                End = window.PointToScreen(Mouse.GetPosition(window));
            }
            else
            {
                DeleteLine();
            }
        }

        public void DeleteLine()
        {
            GPGameDevDialogueEditor.MainWindow window = (GPGameDevDialogueEditor.MainWindow)Application.Current.MainWindow;
            if (window != null && window.MainCanvas.Children.Contains(this)) window.MainCanvas.Children.Remove(this);
            starter.AttachedLinkLine = null;
        }

        private void Line_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeleteLine();
        }
    }
}
