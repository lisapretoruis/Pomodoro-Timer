using System.Windows;
using System.Windows.Threading;

namespace PomodoroTimerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private int _timeLeft; // Time in seconds
        private  int _workDuration = 25 * 60; // 25 minutes
        private  int _breakDuration = 5 * 60; // 5 minutes
        private bool _isWorkSession = true; // Track work/break

        public MainWindow()
        {
            InitializeComponent();

            // Initialize timer
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            // Set initial time
            ResetTimer();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_timeLeft > 0)
            {
                _timeLeft--;
                UpdateTimerDisplay();
            }
            else
            {
                _timer.Stop();

                // Play notification sound
                var player = new System.Media.SoundPlayer("YourSoundFileName.wav");
                player.Play();

                MessageBox.Show(_isWorkSession ? "Time for a break!" : "Back to work!", "Pomodoro Timer");

                // Switch between work and break
                _isWorkSession = !_isWorkSession;
                _timeLeft = _isWorkSession ? _workDuration : _breakDuration;
                UpdateTimerDisplay();
            }
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            ResetTimer();
        }

        private void ResetTimer()
        {
            if (int.TryParse(WorkDurationInput.Text, out int workMinutes) && int.TryParse(BreakDurationInput.Text, out int breakMinutes))
            {
                _workDuration = workMinutes * 60;
                _breakDuration = breakMinutes * 60;
            }
            else
            {
                MessageBox.Show("Invalid input! Please enter valid numbers for durations.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _isWorkSession = true;
            _timeLeft = _workDuration;
            UpdateTimerDisplay();
        }


        private void UpdateTimerDisplay()
        {
            int minutes = _timeLeft / 60;
            int seconds = _timeLeft % 60;
            TimerDisplay.Text = $"{minutes:D2}:{seconds:D2}";
        }


    }
}