using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AnimatedCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Display.Text += button.Content.ToString();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            AnimateDisplayClear();
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text;
                var result = new DataTable().Compute(expression, null);
                AnimateDisplayResult(result.ToString());
            }
            catch
            {
                AnimateDisplayResult("Ошибка");
            }
        }

        private void AnimateDisplayResult(string result)
        {
            var sb = new Storyboard();
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            fadeOut.Completed += (s, e) =>
            {
                Display.Text = result;
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                Display.BeginAnimation(OpacityProperty, fadeIn);
            };
            Display.BeginAnimation(OpacityProperty, fadeOut);
        }

        private void AnimateDisplayClear()
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            fadeOut.Completed += (s, e) =>
            {
                Display.Text = "";
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                Display.BeginAnimation(OpacityProperty, fadeIn);
            };
            Display.BeginAnimation(OpacityProperty, fadeOut);
        }
    }
}
