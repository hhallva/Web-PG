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

        private async Task LoadGamesAsync()
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

        private void GenreComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        { 
            #region Фильтрация игр
            Genre selectedGenre = GenreComboBox.SelectedItem as Genre;

            if (selectedGenre == null || selectedGenre.Name == "Все жанры")
            {
                gamesListView.ItemsSource = _games;
                return;
            }

            gamesListView.ItemsSource = _games.Where(g => g.Genre.Name == selectedGenre.Name).ToList();
            #endregion
        }

        private void GamesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            CreateGameWindow window = new CreateGameWindow(_gameService, _genres);
            window.Show();
            this.Close();
        }
    }
}
