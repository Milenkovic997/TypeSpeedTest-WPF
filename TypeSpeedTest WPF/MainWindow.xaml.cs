using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TypeSpeedTest_WPF
{
    public partial class MainWindow : Window
    {
        private int _counter = -5;
        private int _numWords = 20;
        private int _wordsTyped = 0;
        DispatcherTimer _mainTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _mainTimer.Interval = TimeSpan.FromSeconds(1);
            _mainTimer.Tick += _mainTimer_Tick;
        }

        // TIMER
        private void _mainTimer_Tick(object? sender, EventArgs e)
        {
            _counter++;
            tbTimer.Text = _counter.ToString();
            if (_counter >= 0)
            {
                tbMainInput.IsEnabled = true;
                tbMainInput.Focus();
            }
            else tbMainInput.IsEnabled = false;
        }

        // GET_WORDS
        private void populateWrapPanel()
        {
            wpMain.Children.Clear();
            StreamReader sr = new StreamReader("words.txt");
            string[] words = sr.ReadToEnd().Split('\n');

            Random random = new Random();
            int counter = 0;
            while (counter < _numWords)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = words[random.Next(0, words.Length)],
                    Foreground = Brushes.White,
                    FontSize = 40,
                    FontWeight = FontWeights.Medium,
                    Margin = new Thickness(0, 0, 10, 0),
                };
                wpMain.Children.Add(tb);
                counter++;
            }
        }
        
        // BUTTONS
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnRestart.IsEnabled = false;
            btnStart.IsEnabled = false;
            _mainTimer.Start();
            populateWrapPanel();
        }
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            _counter = -5;
            _wordsTyped = 0;
            tbTimer.Text = "-5";
            wpMain.Children.Clear();
            btnStart.IsEnabled = true;
        }

        // TEXT_CHANGE
        private void tbMainInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text;
            if(tbMainInput.Text.EndsWith(" "))
            {
                text = tbMainInput.Text;
                tbMainInput.Text = "";

                foreach(TextBlock tb in wpMain.Children)
                {
                    if(tb.Foreground == Brushes.White)
                    {
                        if (tb.Text.Equals(text.Replace(" ", "")))
                        {
                            tb.Foreground = Brushes.LightGreen;
                            _wordsTyped++;
                        }
                        else
                        {
                            tb.Foreground = Brushes.Red;
                            _wordsTyped++;
                        }
                        break;
                    }
                }
                if (_wordsTyped == _numWords) calculateResult();
            }
        }

        // RESULT_FUNCTION
        private void calculateResult()
        {
            _mainTimer.Stop();
            double result = 0;
            foreach(TextBlock tb in wpMain.Children)
            {
                if (tb.Foreground == Brushes.LightGreen) result++;
            }
            tbTimer.Text = "Your WPM = " + (int)( result / _counter * 60);

            tbMainInput.IsEnabled = false;
            btnRestart.IsEnabled = true;
        }

        // EXIT_BUTTON_FUNCTIONS
        private void btnExit_MouseEnter(object sender, MouseEventArgs e)
        {
            btnExit.Background = Brushes.Red;
        }
        private void btnExit_MouseLeave(object sender, MouseEventArgs e)
        {
            btnExit.Background = new SolidColorBrush(Color.FromArgb(0xFF, 29, 29, 29));
        }
        private void btnExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void dragFrameFunction(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}