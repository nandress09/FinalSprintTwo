using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WpfTheAionProject.Models
{
    /// <summary>
    /// game map class
    /// </summary>
    public class Map
    {
        #region FIELDS

        private List<Location> _locations;
        private Location _currentLocation;
        private ObservableCollection<Location> _accessibleLocations;

        #endregion

        #region PROPERTIES

        public List<Location> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; }
        }

        public ObservableCollection<Location> AccessibleLocations
        {
            get { return _accessibleLocations; }
            set { _accessibleLocations = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public Map()
        {
            _locations = new List<Location>();
        }

        #endregion

        #region METHODS

        public void Move(Location location)
        {
            _currentLocation = location;
        }

        public bool CanMove(Location location)
        {
            return location.Accessible;
        }
        public string OpenLocationsByRelic(int relicId)
        {
            string message = "This is not the queens relic";
            Location mapLocation = new Location();

            for (int row = 0; row < _maxRows; row++)
            {
                for (int column = 0; column < _maxColumns; column++)
                {
                    mapLocation = _mapLocations[row, column];

                    if (mapLocation != null && mapLocation.RequiredRelicId == relicId)
                    {
                        mapLocation.Accessible = true;
                        message = $"{mapLocation.Name} is now viewable! Go see the queen";
                    }
                }
            }

            return message;
        }


        #endregion
    }
}
