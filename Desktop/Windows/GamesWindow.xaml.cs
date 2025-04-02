using DataLayer.Models;
using DataLayer.Services;
using System.Windows;
using System.Windows.Input;

namespace Desktop.Windows
{
    public partial class GamesWindow : Window
    {
        private readonly GameService _gameService = new();
        private readonly GenreService _genreService = new();

        List<Game> _games = new();
        List<Genre> _genres = new();

        public GamesWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGamesAsync();
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
            => Window_Loaded(sender, e);

        private async Task LoadGamesAsync()
        {
            try
            {
                #region Загрузка и отображение игр
                _games = await _gameService.GetAllAsync();
                gamesListView.ItemsSource = _games;
                #endregion

                #region Загрузка и отображение жанров
                var genres = await _genreService.GetAllAsync();

                _genres.Add(new() { Id = 0, Name = "Все жанры" });
                foreach (var genre in genres)
                    _genres.Add(genre);

                GenreComboBox.ItemsSource = _genres;
                GenreComboBox.SelectedItem = _genres.FirstOrDefault(g => g.Name == "Все жанры");
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Поизошла ошибка загрузке игр: {ex.Message}");
            }

        }

        #region Фильтрация
        private void GenreComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
            => FilterGames();

        private void SearthTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
           => FilterGames();

        private void FilterGames()
        {
            Genre selectedGenre = GenreComboBox.SelectedItem as Genre;
            string searchText = SearthTextBox.Text;

            IEnumerable<Game> filteredGames = _games;

            if (selectedGenre != null && selectedGenre.Name != "Все жанры")
                filteredGames = filteredGames.Where(g => g.Genre.Name == selectedGenre.Name);


            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredGames = filteredGames.Where(g =>
                    g.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    g.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                );
            }

            gamesListView.ItemsSource = filteredGames.ToList();
        }

        //растояние левенштайна
        public static int Calculate(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
                return string.IsNullOrEmpty(t) ? 0 : t.Length;

            if (string.IsNullOrEmpty(t))
                return s.Length;

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++)
                d[i, 0] = i;

            for (int j = 0; j <= m; j++)
                d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
        #endregion

        #region Навигация
        private void GamesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            #region Открытие диалогового окна 
            if (gamesListView.SelectedItem is not Game game)
                return;

            CreateGameWindow window = new CreateGameWindow(game);
            window.ShowDialog();
            Window_Loaded(sender, e);
            #endregion
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            #region Открытие пустого диалогового окна
            CreateGameWindow window = new CreateGameWindow(null);
            window.ShowDialog();
            Window_Loaded(sender, e);
            #endregion
        }
        #endregion

        private void DeleteGameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Game selectedGame = gamesListView.SelectedItem as Game;

            if (selectedGame == null)
            {
                MessageBox.Show("Выберите игру для удаления.");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить игру \"{selectedGame.Name}\" и все ее версии?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);


            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _gameService.DeleteGameAsync(selectedGame.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Поизошла ошибка при удалении игры: {ex.Message}");
                }

                Window_Loaded(sender, e);
            }
        }
    }
}