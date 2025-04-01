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
            if (Game.Id != 0)
            {
                AddGameButton.Visibility = Visibility.Collapsed;
                
                Game = await _gameService.GetAsync(Game.Id);
            }
            else
            {
                SaveGameButton.Visibility = Visibility.Collapsed;
            }

            VersionListView.ItemsSource = Game.GameVersions;
            ReleaseVersionDatePicker.SelectedDate = DateTime.Now;

            DataContext = Game;
            _genres = await _genreService.GetAllAsync();
            GenreComboBox.ItemsSource = _genres;
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddVersionButton_Click(object sender, RoutedEventArgs e)
        {
            #region Проверка на обязательность полей
            StringBuilder errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(NameVersionTextBox.Text))
                errors.AppendLine("Укажите название версии");
            if (String.IsNullOrWhiteSpace(DescriptionVersionTextBox.Text))
                errors.AppendLine("Укажите название версии");
            if (ReleaseVersionDatePicker.SelectedDate is not DateTime)
                errors.AppendLine("Укажите дату публикации версии");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
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
                MessageBox.Show($"Версия созданна успешно.\nСоздан приказ для разработчиков.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}: Поизошла необработанная ошибка при создании новой версии");
            }
            finally
            {
                Window_Loaded(sender, e);
                NameVersionTextBox.Text = "";
                DescriptionTextBox.Text = "";
            }
        }
    }
}
