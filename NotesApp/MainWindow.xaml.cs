using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NotesApp
{
    public partial class MainWindow : Window
    {
        private const string FilePath = "notes.txt";
        private List<string> allNotes = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
            CreateContextMenu();
        }

        // Добавление новой заметки
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NoteTextBox.Text))
            {
                allNotes.Add(NoteTextBox.Text);
                RefreshNotesList(allNotes);
                NoteTextBox.Clear();
            }
        }

        // Выбор заметки для редактирования
        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null)
            {
                NoteTextBox.Text = NotesListBox.SelectedItem.ToString();
            }
        }

        // Сохранение изменений заметки
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                allNotes[selectedIndex] = NoteTextBox.Text;
                RefreshNotesList(allNotes);
            }
        }

        // Удаление заметки
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                allNotes.RemoveAt(selectedIndex);
                RefreshNotesList(allNotes);
                NoteTextBox.Clear();
            }
        }

        // Поиск заметок по содержимому
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchTextBox.Text.ToLower();
            if (query == "поиск...") // чтобы не фильтровалось на пустом плейсхолдере
            {
                RefreshNotesList(allNotes);
                return;
            }

            var filteredNotes = allNotes.Where(n => n.ToLower().Contains(query)).ToList();
            RefreshNotesList(filteredNotes);
        }

        // Помощники для плейсхолдера в поле поиска
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Поиск...")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Поиск...";
                SearchTextBox.Foreground = Brushes.Gray;
            }
        }

        // Загрузка заметок из файла
        private void LoadNotes()
        {
            if (File.Exists(FilePath))
            {
                allNotes = File.ReadAllLines(FilePath).ToList();
                RefreshNotesList(allNotes);
            }
        }

        // Сохранение заметок в файл при выходе
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveNotes();
        }

        private void SaveNotes()
        {
            File.WriteAllLines(FilePath, allNotes);
        }

        private void RefreshNotesList(List<string> notes)
        {
            NotesListBox.ItemsSource = null;
            NotesListBox.ItemsSource = notes;
        }

        // Контекстное меню для изменения цветов
        private void CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem changeWindowColor = new MenuItem { Header = "Изменить цвет фона окна" };
            changeWindowColor.Click += ChangeWindowColor_Click;
            menu.Items.Add(changeWindowColor);

            MenuItem changeTextBoxColor = new MenuItem { Header = "Изменить цвет текстового поля" };
            changeTextBoxColor.Click += ChangeTextBoxColor_Click;
            menu.Items.Add(changeTextBoxColor);

            NotesListBox.ContextMenu = menu;
        }

        private void ChangeWindowColor_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = Color.FromRgb(dlg.Color.R, dlg.Color.G, dlg.Color.B);
                this.Background = new SolidColorBrush(color);
            }
        }

        private void ChangeTextBoxColor_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = Color.FromRgb(dlg.Color.R, dlg.Color.G, dlg.Color.B);
                NoteTextBox.Background = new SolidColorBrush(color);
            }
        }

        private void NotesListBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (NotesListBox.Items.Count == 0)
            {
                e.Handled = true; // запретить открытие меню если нет заметок
            }
        }
    }
}
