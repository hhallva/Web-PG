using DataLayer.Models;
using DataLayer.Services;
using System.IO;
using System.Windows;
using System.Windows.Data;
using Word = Microsoft.Office.Interop.Word;

namespace Desktop.Windows
{
    public partial class CreateGameWindow : Window
    {
        private GameService _gameService;
        List<Genre> _genres;

        public CreateGameWindow(GameService gameService, List<Genre> genres)
        {
            InitializeComponent();

            _gameService = gameService;
            _genres = genres;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenreComboBox.ItemsSource = _genres;
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            GamesWindow window = new GamesWindow();
            window.Show();
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
