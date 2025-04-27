using System.Windows;

namespace GuessNumberGame
{
    public partial class MainWindow : Window
    {
        private GameViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new GameViewModel();
            this.DataContext = viewModel;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CheckGuess(GuessTextBox.Text);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RestartGame();
            GuessTextBox.Clear();
        }
    }
}
