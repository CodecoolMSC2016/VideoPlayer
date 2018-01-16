using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SzerdaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool fullScreen = false;
        private DispatcherTimer DoubleClickTimer = new DispatcherTimer();
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            DoubleClickTimer.Interval = TimeSpan.FromMilliseconds(GetDoubleClickTime());
            DoubleClickTimer.Tick += (s, e) => DoubleClickTimer.Stop();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan))
            {
                slider.Minimum = 0;
                slider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                slider.Value++;
                minLabel.Content = String.Format("{0}:{1}:{2} / {3}:{4}:{5}", mediaPlayer.Position.ToString(@"hh"), mediaPlayer.Position.ToString(@"mm"), mediaPlayer.Position.ToString(@"ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"hh"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"ss"));
            }
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            timer.Start();
        }

        private void Pause_Button_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            timer.Stop();
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            slider.Value = 0;
            timer.Stop();
        }

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan tp = new TimeSpan(0);
                mediaPlayer.Position = tp;
                Stop_Button_Click(sender, e);
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.DefaultExt = ".WMV";
                openFileDialog.Filter = "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                  " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";

                Nullable<bool> result = openFileDialog.ShowDialog();
                string path = openFileDialog.FileName;

                if (result == true)
                {  
                    mediaPlayer.Source = new Uri(openFileDialog.FileName);
                    Thread.Sleep(700);
                    Play_Button_Click(sender, e);
                    minLabel.Content = String.Format("{0}:{1}:{2} / {3}:{4}:{5}", mediaPlayer.Position.ToString(@"hh"), mediaPlayer.Position.ToString(@"mm"), mediaPlayer.Position.ToString(@"ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"hh"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"ss"));
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error.");
            }
        }

        private void mediaPlayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!DoubleClickTimer.IsEnabled)
            {
                DoubleClickTimer.Start();
            }
            else
            {
                if (!fullScreen)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                    playButton.Visibility = Visibility.Hidden;
                    pauseButton.Visibility = Visibility.Hidden;
                    stopButton.Visibility = Visibility.Hidden;
                    openButton.Visibility = Visibility.Hidden;
                    slider.Visibility = Visibility.Hidden;
                    minLabel.Visibility = Visibility.Hidden;
                    durationLabel.Visibility = Visibility.Hidden;
                    ForwardButton.Visibility = Visibility.Hidden;
                    RewindButton.Visibility = Visibility.Hidden;
                    mediaPlayer.Margin = new Thickness(0, 0, 0, 0);
                    this.ResizeMode = ResizeMode.NoResize;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                    playButton.Visibility = Visibility.Visible;
                    pauseButton.Visibility = Visibility.Visible;
                    stopButton.Visibility = Visibility.Visible;
                    openButton.Visibility = Visibility.Visible;
                    slider.Visibility = Visibility.Visible;
                    minLabel.Visibility = Visibility.Visible;
                    durationLabel.Visibility = Visibility.Visible;
                    ForwardButton.Visibility = Visibility.Visible;
                    RewindButton.Visibility = Visibility.Visible;
                    mediaPlayer.Margin = new Thickness(33, 10, 36, 133);
                }

                fullScreen = !fullScreen;
            }
        }

        [DllImport("user32")]
        private static extern uint GetDoubleClickTime();

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(slider.Value);
        }

        private void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            slider.Value -= 10;
            mediaPlayer.Position = TimeSpan.FromSeconds(slider.Value);
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            slider.Value += 10;
            mediaPlayer.Position = TimeSpan.FromSeconds(slider.Value);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)VolumeSlider.Value;
        }
    }
}
