using System;
using System.ComponentModel;

namespace GuessNumberGame
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private int _targetNumber;
        private int _attempts;
        private string _hintMessage;
        private string _resultMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public GameViewModel()
        {
            RestartGame();
        }

        public string HintMessage
        {
            get => _hintMessage;
            set
            {
                _hintMessage = value;
                OnPropertyChanged(nameof(HintMessage));
            }
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set
            {
                _resultMessage = value;
                OnPropertyChanged(nameof(ResultMessage));
            }
        }

        public int Attempts
        {
            get => _attempts;
            set
            {
                _attempts = value;
                OnPropertyChanged(nameof(Attempts));
            }
        }

        public void CheckGuess(string userInput)
        {
            if (int.TryParse(userInput, out int guess))
            {
                if (guess < 1 || guess > 100)
                {
                    ResultMessage = "Введите число от 1 до 100!";
                    return;
                }

                Attempts++;

                if (guess < _targetNumber)
                {
                    ResultMessage = "Слишком маленькое!";
                }
                else if (guess > _targetNumber)
                {
                    ResultMessage = "Слишком большое!";
                }
                else
                {
                    ResultMessage = $"Поздравляем! Вы угадали за {Attempts} попыток!";
                }
            }
            else
            {
                ResultMessage = "Ошибка ввода! Введите число.";
            }
        }

        public void RestartGame()
        {
            Random rand = new Random();
            _targetNumber = rand.Next(1, 101);
            Attempts = 0;
            HintMessage = "Угадайте число от 1 до 100";
            ResultMessage = "";
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
