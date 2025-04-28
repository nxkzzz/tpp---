using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        private string currentFilePath = null;

        public MainWindow()
        {
            InitializeComponent();
            // Привязка горячих клавиш
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewCommand_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenCommand_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveCommand_Executed));

            InputBindings.Add(new KeyBinding(ApplicationCommands.New, new KeyGesture(Key.N, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Open, new KeyGesture(Key.O, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Save, new KeyGesture(Key.S, ModifierKeys.Control)));
        }

        private void NewCommand_Executed(object sender, RoutedEventArgs e)
        {
            TextBoxEditor.Clear();
            currentFilePath = null;
        }

        private void OpenCommand_Executed(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    TextBoxEditor.Text = File.ReadAllText(openFileDialog.FileName);
                    currentFilePath = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveCommand_Executed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentFilePath))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        currentFilePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return; // Отмена сохранения
                    }
                }

                File.WriteAllText(currentFilePath, TextBoxEditor.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

