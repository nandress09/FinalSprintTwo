using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTheAionProject.Models;
using WpfTheAionProject;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using WpfTheAionProject.DataLayer;

namespace WpfTheAionProject.PresentationLayer
{
    /// <summary>
    /// view model for the game session view
    /// </summary>
    public class GameSessionViewModel : ObservableObject
    {
        #region ENUMS

        #endregion

        #region FIELDS

        private DateTime _gameStartTime;
        private string _gameTimeDisplay;
        private TimeSpan _gameTime;

        private Player _player;

        private Map _gameMap;
        private Location _currentLocation;
        private ObservableCollection<Location> _accessibleLocations;
        private string _currentLocationName;

        #endregion

        #region PROPERTIES

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string MessageDisplay
        {
            get { return _currentLocation.Message; }
        }
        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
            }
        }

        public ObservableCollection<Location> AccessibleLocations
        {
            get
            {
                return _accessibleLocations;
            }
            set
            {
                _accessibleLocations = value;
                OnPropertyChanged(nameof(AccessibleLocations));
            }
        }

        public string CurrentLocationName
        {
            get { return _currentLocationName; }
            set
            {
                _currentLocationName = value;
                OnPlayerMove();
                OnPropertyChanged("CurrentLocation");
            }
        }

        public string MissionTimeDisplay
        {
            get { return _gameTimeDisplay; }
            set
            {
                _gameTimeDisplay = value;
                OnPropertyChanged(nameof(MissionTimeDisplay));
            }
        }

        #endregion

        #region CONSTRUCTORS

        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(
            Player player,
            Map gameMap)
        {
            _player = player;
            _gameMap = gameMap;

            _currentLocation = _gameMap.CurrentLocation;
            _accessibleLocations = new ObservableCollection<Location>();

            InitializeView();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// initial setup of the game session view
        /// </summary>
        private void InitializeView()
        {
            _gameStartTime = DateTime.Now;
            GameTimer();

            UpdateAccessibleLocations();
        }

        /// <summary>
        /// player move event handler
        /// </summary>
        private void OnPlayerMove()
        {
            //
            // set new current location
            //
            foreach (Location location in AccessibleLocations)
            {
                if (location.Name == _currentLocationName)
                {
                    _currentLocation = location;
                }
            }
            //_currentLocation = AccessibleLocations.FirstOrDefault(l => l.Name == _currentLocationName);

            //
            // update stats if player has not visited the location
            //
            if (!_player.HasVisited(_currentLocation))
            {
                //
                // update list of visited locations
                //
                _player.LocationsVisited.Add(_currentLocation);

                //
                // update player experience points
                //
                _player.ExperiencePoints += _currentLocation.ModifiyExperiencePoints;

                //
                // update player health
                //
                if (_currentLocation.ModifyHealth != 0)
                {
                    _player.Health += _currentLocation.ModifyHealth;
                    if (_player.Health > 100)
                    {
                        _player.Health = 100;
                        _player.Lives++;
                    }
                }

                //
                // update player lives
                //
                if (_currentLocation.ModifyLives != 0) _player.Lives += _currentLocation.ModifyLives;
            }

            //
            // display a new message if available
            //
            OnPropertyChanged(nameof(MessageDisplay));


            //
            // update the list of locations
            //
            UpdateAccessibleLocations();
        }

        /// <summary>
        /// update the accessible locations for the list box
        /// </summary>
        private void UpdateAccessibleLocations()
        {
            //
            // reset accessible locations list
            //
            _accessibleLocations.Clear();

            //
            // add all accessible locations to list
            //
            foreach (Location location in _gameMap.Locations)
            {
                if (
                    location.Accessible == true ||
                    _player.ExperiencePoints >= location.RequiredExperiencePoints)
                {
                    _accessibleLocations.Add(location);
                }
            }

            //
            // remove current location
            //
            _accessibleLocations.Remove(_accessibleLocations.FirstOrDefault(l => l == _currentLocation));

            //
            // notify list box in view to update
            //
            OnPropertyChanged(nameof(AccessibleLocations));
        }


        #region GAME TIMER METHODS

        /// <summary>
        /// running time of game
        /// </summary>
        /// <returns></returns>
        private TimeSpan GameTime()
        {
            return DateTime.Now - _gameStartTime;
        }

        /// <summary>
        /// game time event, publishes every 1 second
        /// </summary>
        public void GameTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += OnGameTimerTick;
            timer.Start();
        }

        /// <summary>
        /// game timer event handler
        /// 1) update mission time on window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddItemToInventory()
        {
            //
            // confirm a game item selected and is in current location
            // subtract from location and add to inventory
            //
            if (_currentGameItem != null && _currentLocation.GameItems.Contains(_currentGameItem))
            {
                //
                // cast selected game item 
                //
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.RemoveGameItemQuantityFromLocation(selectedGameItemQuantity);
                _player.AddGameItemQuantityToInventory(selectedGameItemQuantity);

                OnPlayerPickUp(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// remove item from the players inventory
        /// </summary>
        /// <param name="selectedItem"></param>
        public void RemoveItemFromInventory()
        {
            //
            // confirm a game item selected and is in inventory
            // subtract from inventory and add to location
            //
            if (_currentGameItem != null)
            {
                //
                // cast selected game item 
                //
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.AddGameItemQuantityToLocation(selectedGameItemQuantity);
                _player.RemoveGameItemQuantityFromInventory(selectedGameItemQuantity);

                OnPlayerPutDown(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// process events when a player picks up a new game item
        /// </summary>
        /// <param name="gameItemQuantity">new game item</param>
        private void OnPlayerPickUp(GameItemQuantity gameItemQuantity)
        {
            _player.ExperiencePoints += gameItemQuantity.GameItem.ExperiencePoints;
            _player.Wealth += gameItemQuantity.GameItem.Value;
        }

        /// <summary>
        /// process events when a player puts down a new game item
        /// </summary>
        /// <param name="gameItemQuantity">new game item</param>
        private void OnPlayerPutDown(GameItemQuantity gameItemQuantity)
        {
            _player.Wealth -= gameItemQuantity.GameItem.Value;
        }

        /// <summary>
        /// process using an item in the player's inventory
        /// </summary>
        public void OnUseGameItem()
        {
            switch (_currentGameItem.GameItem)
            {
                case Potion potion:
                    ProcessPotionUse(potion);
                    break;
                case Relic relic:
                    ProcessRelicUse(relic);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// process the effects of using the relic
        /// </summary>
        /// <param name="potion">potion</param>
        private void ProcessRelicUse(Relic relic)
        {
            string message;

            switch (relic.UseAction)
            {
                case Relic.UseActionType.OPENLOCATION:
                    message = _gameMap.OpenLocationsByRelic(relic.Id);
                    CurrentLocationInformation = relic.UseMessage;
                    break;
                case Relic.UseActionType.KILLPLAYER:
                    OnPlayerDies(relic.UseMessage);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// process the effects of using the potion
        /// </summary>
        /// <param name="potion">potion</param>
        private void ProcessPotionUse(Potion potion)
        {
            _player.Health += potion.HealthChange;
            _player.Lives += potion.LivesChange;
            _player.RemoveGameItemQuantityFromInventory(_currentGameItem);
        }

        /// <summary>
        /// process player dies with option to reset and play again
        /// </summary>
        /// <param name="message">message regarding player death</param>
        private void OnPlayerDies(string message)
        {
            string messagetext = message +
                "\n\nWould you like to play again?";

            string titleText = "Death";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messagetext, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuiteApplication();
                    break;
            }
        }


        #endregion

        #endregion

        #region EVENTS



        #endregion
    }

}
