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


namespace SampleChallenge {
    /// <summary>
    /// Simple WPF showing functionality of key components.
    /// </summary>
    public partial class MainWindow : Window {
        //Timer used for updating video slider
        System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow() {
            InitializeComponent();
            //initialize timer/add tick event
            dTimer.Tick += DTimer_Tick;
            dTimer.Interval = TimeSpan.FromMilliseconds(200);
            //clear textbox
            richTextBox.Document.Blocks.Clear();
            richTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //fake start video to get thumbnail
            video.Play();
            video.Pause();
        }
        #region Loading/Unloading
        /// <summary>
        /// Contains loading and unloading events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //nothing needed here at this time
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            //do something here if mediaelement and image didn't already do it
        }
        #endregion
        #region Random Button Clicks
        /// <summary>
        /// Contains events for misc. button clicks.
        /// I.e button clicks that only log in rtb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e) {
            //use current button tag to identify what has been pressed
            Button temp = (Button)sender;
            string time = System.DateTime.Now.TimeOfDay.ToString();
            //write press and time to rtf, scroll to bottom
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(temp.Tag + " " + time)));
            richTextBox.ScrollToEnd();
        }
        #endregion
        #region Media Play
        /// <summary>
        /// Contains events for playing media.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediaButton_Click(object sender, RoutedEventArgs e) {
            ///Button click event for the play pause button
            Button temp = (Button)sender;
            //determine which it is based upon the tag
            if (temp.Tag.ToString() == "Play") {  //play button
                //play video
                video.Play();
                //start timer
                dTimer.Start();
                //log to rtf
                string time = System.DateTime.Now.TimeOfDay.ToString();
                richTextBox.Document.Blocks.Add(new Paragraph(new Run("Video Play " + time)));
                richTextBox.ScrollToEnd();
            }
            else { //pause button
                //pause video
                dTimer.Stop();
                video.Pause();
                //log to rtf
                string time = System.DateTime.Now.TimeOfDay.ToString();
                richTextBox.Document.Blocks.Add(new Paragraph(new Run("Video Pause " + time)));
                richTextBox.ScrollToEnd();
            }
        }
        /// <summary>
        /// Timer Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DTimer_Tick(object sender, EventArgs e) {
            //if button is not pressed
            if (Mouse.LeftButton != MouseButtonState.Pressed) {
                //set slider value to position of video
                slider.Value = (video.Position.TotalMilliseconds / video.NaturalDuration.TimeSpan.TotalMilliseconds) * 100;
            }
            else { //if button is pressed
                //set video position to that of the slider
                video.Position = TimeSpan.FromMilliseconds((slider.Value / 100) * video.NaturalDuration.TimeSpan.TotalMilliseconds);
                string log = "Video Position Set: " + video.Position.TotalSeconds;
                richTextBox.Document.Blocks.Add(new Paragraph(new Run(log)));
                richTextBox.ScrollToEnd();
            }
        }
        /// <summary>
        /// When the video reaches the end of play, replay it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void video_MediaEnded(object sender, RoutedEventArgs e) {
            //reset video position, and resume play
            video.Position = TimeSpan.Zero;
            video.Play();
        }
        #endregion
        #region Checkbox Events
        /// <summary>
        /// Contains all events for check boxes such as 
        /// toggling visibility for core components, and logging
        /// interactions to RTB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Checkbox1_Checked(object sender, RoutedEventArgs e) {
            VideoGrid.Visibility = Visibility.Hidden; //hides the video
        }

        private void Checkbox1_Unchecked(object sender, RoutedEventArgs e) {
            VideoGrid.Visibility = Visibility.Visible; //reveals the video
        }
        private void Checkbox2_Checked(object sender, RoutedEventArgs e) {
            PictureGrid.Visibility = Visibility.Hidden;  //hides the picture
        }

        private void Checkbox2_Unchecked(object sender, RoutedEventArgs e) {
            PictureGrid.Visibility = Visibility.Visible; //reveals the picture
        }
        private void Checkbox3_Checked(object sender, RoutedEventArgs e) {
            richTextBox.Visibility = Visibility.Hidden; //hides the text
        }

        private void Checkbox3_Unchecked(object sender, RoutedEventArgs e) {
            richTextBox.Visibility = Visibility.Visible; //reveals the text
        }
        /// <summary>
        /// Logs the event of selecting one of the check boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_Click(object sender, RoutedEventArgs e) {
            //writes appropriate data to log.
            CheckBox temp = (CheckBox)sender;
            string time = System.DateTime.Now.TimeOfDay.ToString();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(temp.Tag + " " + time)));
            richTextBox.ScrollToEnd();
        }
        #endregion
        #region Mouse Event Logging
        /// <summary>
        /// Contains events for tracking mouse position
        /// and logging mouse clicks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            //logs data for pressing down on the mouse
            Point tempPos = Mouse.GetPosition(Application.Current.MainWindow); //gets mouse pos
            string time = System.DateTime.Now.TimeOfDay.ToString(); //gets time
            string mousepos = "Mouse Down. (x,y): (" + tempPos.ToString() + ") Time: " + time;
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(mousepos))); //logs
            richTextBox.ScrollToEnd();
        }
        private void Window_MouseMove(object sender, MouseEventArgs e) {
            //displays mouse position
            Point tempPos = Mouse.GetPosition(Application.Current.MainWindow);
            string mousepos = "Mouse Pos: (" + tempPos.ToString() + ")";
            mousePos.Text = mousepos;
        }
        #endregion
    }
}
