using DataLayer.Models;
using DataLayer.Services;
using DocumentsLibrary;
using System.Text;
using System.Windows;

namespace Desktop.Windows
{
    public partial class CreateGameWindow : Window
    {
        private GameService _gameService = new();
        private GenreService _genreService = new();
        private VersionsService _versionsService = new();

        private Game _game;
        public Game Game { get => _game; set => _game = value; }

        List<Genre> _genres;

        public CreateGameWindow(Game? game = null)
        {
            InitializeComponent();

            Game = game ?? new();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Game.Id != 0)
                {
                    #region Форма и отображение элементов
                    Title = "Редактирование игры";

                    NameVersionTextBox.Text = String.Empty;
                    DescriptionVersionTextBox.Text = String.Empty;

                    AddGameButton.Visibility = Visibility.Collapsed;
                    SaveGameButton.Visibility = Visibility.Visible;
                    AddVersionButton.Visibility = Visibility.Visible;
                    #endregion

                    Game = await _gameService.GetAsync(Game.Id);
                }
                else
                {
                    #region Форма и отображение элементов
                    Title = "Добавление игры";

                    AddGameButton.Visibility = Visibility.Visible;
                    SaveGameButton.Visibility = Visibility.Collapsed;
                    AddVersionButton.Visibility = Visibility.Collapsed;

                    NameVersionTextBox.Text = "v1";
                    DescriptionVersionTextBox.Text = "Самая первая версия игры.";
                    ReleaseVersionDatePicker.SelectedDate = DateTime.Now;
                    #endregion
                }
                DataContext = Game;

                #region Отображение версий
                VersionListView.ItemsSource = Game.GameVersions;
                #endregion

                #region Отображение жанров
                _genres = await _genreService.GetAllAsync();
                GenreComboBox.ItemsSource = _genres;
                GenreComboBox.SelectedItem = Game.Genre;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Поизошла ошибка при загрузке информации об игр: {ex.Message}");
            }
        }

        private async void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            #region Проверка на обязательность полей
            if (CheckGameData())
                return;
            if (CheckVersionData())
                return;
            #endregion

            try
            {
                Game.GenreId = (GenreComboBox.SelectedItem as Genre).Id;
                Game = await _gameService.AddGameAsync(Game);
                AddVersionButton_Click(sender, e);

                Window_Loaded(sender, e);
                MessageBox.Show($"Создание произошло успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Поизошла ошибка при сохранении: {ex.Message}");
            }
        }

        private async void SaveGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Game.GenreId = (GenreComboBox.SelectedItem as Genre).Id;
                await _gameService.UpdateAsync(Game);
                MessageBox.Show($"Сохранение произошло успешно!", "Сообщение.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Поизошла ошибка при сохранении: {ex.Message}", "Сообщение.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddVersionButton_Click(object sender, RoutedEventArgs e)
        {
            #region Проверка на обязательность полей
            if (CheckVersionData())
                return;
            #endregion

            #region Создание объектов
            GameVersion version = new()
            {
                Id = 0,
                GameId = Game.Id,
                Version = NameVersionTextBox.Text.Trim(),
                Description = DescriptionVersionTextBox.Text.Trim(),
                PublicationDate = ReleaseVersionDatePicker.DisplayDate
            };

            string fileName = $"C:\\Temp\\Docs\\Приказ-{Game.Name.Replace(" ", "_")}-{NameVersionTextBox.Text.Replace(" ", "_")}.pdf";
            string title = $"Обвленние {NameVersionTextBox.Text.Trim()} игры {Game.Name}";
            string body = $"Добрый день, коллеги.\nВы назначены разработчиком версии {NameVersionTextBox.Text.Trim()} новой игры \"{Game.Name}\"! Просим ознакомиться с приказом и приступить к работе.\nС уважением, HR-специалист.";
            #endregion

            try
            {
                await _versionsService.AddVersionAsync(version);

                using (WordService word = new())
                    word.CreatePdf(fileName, title, body);
                MessageBox.Show($"Запрос на новую версию создан!\nСоздан приказ для разработчиков.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Поизошла необработанная ошибка при создании новой версии: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Window_Loaded(sender, e);

                NameVersionTextBox.Text = "";
                DescriptionTextBox.Text = "";
            }
        }

        private bool CheckGameData()
        {
            StringBuilder errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(NameTextBox.Text))
                errors.AppendLine("Укажите название новой игры");
            if (String.IsNullOrWhiteSpace(DescriptionVersionTextBox.Text))
                errors.AppendLine("Укажите описание новой игры");
            if (GenreComboBox.SelectedItem is not Genre)
                errors.AppendLine("Выберите жанр новой игры");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return true;
            }
            return false;
        }

        private bool CheckVersionData()
        {
            StringBuilder errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(NameVersionTextBox.Text))
                errors.AppendLine("Укажите название новой версии");
            if (String.IsNullOrWhiteSpace(DescriptionVersionTextBox.Text))
                errors.AppendLine("Укажите описание новой версии");
            if (ReleaseVersionDatePicker.SelectedDate is not DateTime)
                errors.AppendLine("Укажите дату публикации новой версии");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return true;
            }
            return false;
        }
    }
}
