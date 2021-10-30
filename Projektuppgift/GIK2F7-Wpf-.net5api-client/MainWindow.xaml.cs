using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataHandler dataHandler = new();
        private ObservableCollection<Game> gamesList = new();
        private Game _gameSelected = new Game();
        private string imageBase64;

        /// <summary>
        /// Initializes components for the window and binds gamesList and the listbox element.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            gameListBox.ItemsSource = gamesList;
        }

        /// <summary>
        /// Hides buttons in the ui.
        /// </summary>
        public void HideButtons()
        {
            if (updateButton.Visibility == Visibility.Visible || editButton.Visibility == Visibility.Visible || deleteButton.Visibility == Visibility.Visible)
            {
                gameShowcaseTitle.Visibility = Visibility.Visible;
                gameEditTitle.Visibility = Visibility.Collapsed;

                gameShowcaseRating.Visibility = Visibility.Visible;
                gameEditRating.Visibility = Visibility.Collapsed;

                gameShowcaseDescription.Visibility = Visibility.Visible;
                gameEditDescription.Visibility = Visibility.Collapsed;

                editButton.Visibility = Visibility.Hidden;
                deleteButton.Visibility = Visibility.Hidden;
                updateButton.Visibility = Visibility.Hidden;
                editPicture.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Loads a picture for when you want to add a game.
        /// </summary>
        public void ButtonClickAddPicture(object sender, RoutedEventArgs e)
        {
            string fileName = dataHandler.AddImage(out imageBase64);
            if(fileName != null)
            {
                pictureAddSource.Text = fileName;
            }
            else
            {
                MessageBox.Show("Please choose a file.");
            }
        }

        /// <summary>
        /// Adds a game into the database and list.
        /// </summary>
        public async void ButtonClickAddGame(object sender, RoutedEventArgs e)
        {
            if(title.Text.Length > 0 && description.Text.Length > 0 && rating.Text.Length > 0)
            {
                bool Input = float.TryParse(rating.Text, out float Parse);
                if (Input)
                {
                    if(Parse > 0 && Parse <= 10)
                    {
                        if (imageBase64 != null)
                        {
                            var list = await dataHandler.AddGameAsync(title.Text, description.Text, Parse, imageBase64);
                            UpdateList(list);
                        }
                        else
                        {
                            var list = await dataHandler.AddGameAsync(title.Text, description.Text, Parse, dataHandler.defaultImage);
                            UpdateList(list);
                        }

                        MessageBox.Show($"Added game Title: {title.Text} Description: {description.Text} Rating: {rating.Text}");
                    }
                    else
                    {
                        MessageBox.Show("Enter a valid rating.");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to add game, make sure that every field is filled.");
                }
            }
               
        }

        /// <summary>
        /// Connects to the database and loads the data.
        /// </summary>
        public async void ButtonClickConnect(object sender, RoutedEventArgs e)
        {
            HideButtons();

            ObservableCollection<Game> games = await dataHandler.GetData();

            UpdateList(games);
        }

        /// <summary>
        /// Displays data when clicking an element in the listbox.
        /// </summary>
        public void GameListBoxLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if(gameListBox.Items.Count > 0)
            {
                if(gameListBox.SelectedIndex != -1)
                {
                    _gameSelected = (Game)gameListBox.Items.GetItemAt(gameListBox.SelectedIndex);

                    gameShowcaseTitle.Text = _gameSelected.name;
                    gameShowcaseDescription.Text = _gameSelected.description;
                    gameShowcaseRating.Text = _gameSelected.rating.ToString();
                    if(_gameSelected.image == null)
                    {
                        BitmapImage image = dataHandler.Base64StringToBitmapImage(dataHandler.defaultImage);
                        imageShowcase.Source = image;
                    }
                    else
                    {
                        BitmapImage image = dataHandler.Base64StringToBitmapImage(_gameSelected.image);
                        imageShowcase.Source = image;
                    }

                    gameEditTitle.Text = _gameSelected.name;
                    gameEditRating.Text = _gameSelected.rating.ToString();
                    gameEditDescription.Text = _gameSelected.description;

                    editButton.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Visible;
                    
                }
            }
        }

        /// <summary>
        /// Updates a user selected from the lixtbox and updates it in the database.
        /// </summary>
        public async void ButtonClickUpdate(object sender, RoutedEventArgs e)
        {
            _gameSelected = (Game)gameListBox.Items.GetItemAt(gameListBox.SelectedIndex);

            int id = _gameSelected.id;
            int listId = gameListBox.SelectedIndex;

            if (gameEditTitle.Text.Length > 0 && gameEditDescription.Text.Length > 0 && gameEditRating.Text.Length > 0)
            {
                bool Input = float.TryParse(gameEditRating.Text, out float Parse);
                if (Input && Parse > 0 && Parse <= 10) 
                {
                    if (imageBase64 != null || _gameSelected.image != null)
                    {
                        var list = await dataHandler.UpdateGameAsync(listId, id, gameEditTitle.Text, gameEditDescription.Text, Parse, imageBase64);
                        UpdateList(list);
                    }
                    else
                    {
                        var list = await dataHandler.UpdateGameAsync(listId, id, gameEditTitle.Text, gameEditDescription.Text, Parse, dataHandler.defaultImage);
                        UpdateList(list);
                    }

                    gameShowcaseTitle.Text = gameEditTitle.Text;
                    gameShowcaseDescription.Text = gameEditDescription.Text;
                    gameShowcaseRating.Text = Parse.ToString();


                    //FUNGERAR INTE ATT SKICKA LISTAN, LÄR LOOPA EN LISTA IN I GAMESLIST ISTÄLLET?
                    ButtonClickEdit(sender, e);                }
                else
                {
                    MessageBox.Show("Failed to edit game, make sure that the rating is a valid number.");
                }
            }
            else
            {
                MessageBox.Show("Failed to add game, enter a valid rating.");
            }
        }

        /// <summary>
        /// Opens "edit mode" where you can edit your selected item in the listbox .
        /// </summary>
        private void ButtonClickEdit(object sender, RoutedEventArgs e)
        {
            if (gameEditTitle.Visibility == Visibility.Collapsed || gameEditRating.Visibility == Visibility.Collapsed || gameEditDescription.Visibility == Visibility.Collapsed || updateButton.Visibility == Visibility.Hidden)
            {
                gameShowcaseTitle.Visibility = Visibility.Collapsed;
                gameEditTitle.Visibility = Visibility.Visible;

                gameShowcaseRating.Visibility = Visibility.Collapsed;
                gameEditRating.Visibility = Visibility.Visible;

                gameShowcaseDescription.Visibility = Visibility.Collapsed;
                gameEditDescription.Visibility = Visibility.Visible;

                updateButton.Visibility = Visibility.Visible;
                editPicture.Visibility = Visibility.Visible;
            }
            else
            {
                gameShowcaseTitle.Visibility = Visibility.Visible;
                gameEditTitle.Visibility = Visibility.Collapsed;

                gameShowcaseRating.Visibility = Visibility.Visible;
                gameEditRating.Visibility = Visibility.Collapsed;

                gameShowcaseDescription.Visibility = Visibility.Visible;
                gameEditDescription.Visibility = Visibility.Collapsed;

                updateButton.Visibility = Visibility.Hidden;
                editPicture.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Applies the filter to the listbox to filter out what is not needed.
        /// </summary>
        /// <returns>bool</returns>
        /// https://www.wpf-tutorial.com/listview-control/listview-filtering/
        private bool GameFilter(object pItem)
        {
            if (string.IsNullOrEmpty(searchBox.Text))
            {
                return true;
            }
            else
            {
                return ((pItem as Game).name.IndexOf(searchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        /// <summary>
        /// Checks if the text in the search box has changed and then sets the filter to the text.
        /// </summary>
        private void searchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(gameListBox.ItemsSource);
            view.Filter = GameFilter;
        }

        /// <summary>
        /// Adds a picture for when you want to edit the picture on the selected item.
        /// </summary>
        private void ButtonClickEditPicture(object sender, RoutedEventArgs e)
        {
            string fileName = dataHandler.AddImage(out imageBase64);
            if (fileName != null)
            {
                imageShowcase.Source = dataHandler.Base64StringToBitmapImage(imageBase64);
                //pictureAddSource.Text = fileName;
            }
            else
            {
                MessageBox.Show("Please choose a file.");
            }
        }

        /// <summary>
        /// Deletes the currently selected item from the listbox and database.
        /// </summary>
        private async void ButtonClickDelete(object sender, RoutedEventArgs e)
        {
            _gameSelected = (Game)gameListBox.Items.GetItemAt(gameListBox.SelectedIndex);

            int id = _gameSelected.id;
            int listId = gameListBox.SelectedIndex;

            var list = await dataHandler.DeleteGameAsync(listId, id);
            UpdateList(list);
            HideButtons();
        }

        /// <summary>
        /// Updates the gamelist to allow for changes in the listbox.
        /// </summary>
        /// <param name="pGamesList">List of games</param>
        private void UpdateList(ObservableCollection<Game> pGamesList)
        {
            gamesList.Clear();

            foreach(var game in pGamesList)
            {
                gamesList.Add(game); 
            }
        }
    }
}
