using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTheAionProject.PresentationLayer;
using WpfTheAionProject.DataLayer;
using WpfTheAionProject.Models;
using System.Collections.ObjectModel;

namespace WpfTheAionProject.BusinessLayer
{
    public class GameBusiness
    {
        bool _newPlayer = false;

        GameSessionViewModel _gameSessionViewModel;
        Player _player = new Player();
        PlayerSetupView _playerSetupView;
        Map _gameMap;

        public GameBusiness()
        {
            SetupPlayer();
            InitializeDataSet();
            InstantiateAndShowView();
        }
        
        private void SetupPlayer()
        {
            if (_newPlayer)
            {
                _playerSetupView = new PlayerSetupView(_player);
                _playerSetupView.ShowDialog();

                //
                // setup up game based player properties
                //
                _player.ExperiencePoints = 0;
                _player.Health = 100;
                _player.Lives = 3;
            }
            else
            {
                _player = GameData.PlayerData();
            }
        }

        /// <summary>
        /// initialize data set
        /// </summary>
        private void InitializeDataSet()
        {
            _player = GameData.PlayerData();
            _gameMap = GameData.GameMap();
        }

        /// <summary>
        /// create view model with data set
        /// </summary>
        private void InstantiateAndShowView()
        {
            //
            // instantiate the view model and initialize the data set
            //
            _gameSessionViewModel = new GameSessionViewModel(
                _player,
                _gameMap
                );
            GameSessionView gameSessionView = new GameSessionView(_gameSessionViewModel);

            gameSessionView.DataContext = _gameSessionViewModel;

            gameSessionView.Show();

            //
            // dialog window is initially hidden to mitigate issue with
            // main window closing after dialog window closes
            //
            // commented out because the player setup window is disabled
            //
            //_playerSetupView.Close();
        }
    }
}
