using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Game.Configs;
using Location.Configs;
using Persistence.Interfaces;
using UnityEngine;
using Zenject;
using ZLinq;

namespace Location
{
    //todo refactor
    public class LocationService : MonoBehaviour
    {
        [SerializeField] private List<LocationBehaviour> locations;
        
        private LocationBehaviour _currentLocation;
        
        private ICreateRepository<LocationDataDto> _writeService;
        private IDataService<LocationDataDto> _dataService;
        
        private readonly List<LocationId> _locationIds =
            Enum.GetValues(typeof(LocationId)).Cast<LocationId>().ToList();
        
        private int _chosenLocationIndex;

        [Inject]
        public void Construct(IDataService<LocationDataDto> dataService)
        {
            _dataService = dataService;
        }
        
        private void DisableAllLocations()
        {
            foreach (var beh in locations)
            {
                beh.gameObject.SetActive(false);
            }
        }

        public void EnableLocation(LocationId id)
        {
            DisableAllLocations();
            _currentLocation = locations
                .AsValueEnumerable()
                .First(x => x.LocationConfig.LocationId == id);
            
            _currentLocation.gameObject.SetActive(true);
        }

        public DifficultyConfig GetLocationDifficulty => _currentLocation.DifficultyConfig;
        
        
        public LocationId SetPreviousLocation()
        {
            _chosenLocationIndex--;
            if (_chosenLocationIndex < 0) _chosenLocationIndex = _locationIds.Count - 1;

            var id = _locationIds[_chosenLocationIndex];

            var data = _dataService.GetData();
            if(data.Unlocked.AsValueEnumerable().Contains(id))
            {
                data.CurrentLocationIndex = data.Unlocked.IndexOf(id);
            }
            return id;
        }
        
        public LocationId SetNextLocation()
        {
            _chosenLocationIndex++;
            if (_chosenLocationIndex >= _locationIds.Count) _chosenLocationIndex = 0;
            var id = _locationIds[_chosenLocationIndex];
            
            var data = _dataService.GetData();
            if(data.Unlocked.AsValueEnumerable().Contains(id))
            {
                data.CurrentLocationIndex = data.Unlocked.IndexOf(id);
            }
            
            return id;
        }

        public void UnlockLocation(LocationId id)
        {
            
            var data = _dataService.GetData();
            data.Unlocked.Add(id);
            data.CurrentLocationIndex = data.Unlocked.IndexOf(id);
        }

        public LocationConfig GetActiveLocationConfig() => GetLocationConfig(_locationIds[_chosenLocationIndex]);

        public LocationConfig GetLocationConfig(LocationId id)
        {
            return locations
                .First(x => x.LocationConfig.LocationId == id)
                .LocationConfig;
        }

        public bool IsChosenLocationLocked() => !_dataService.GetData().Unlocked.AsValueEnumerable()
            .Contains(_locationIds[_chosenLocationIndex]);
        public LocationId UnlockedLocationId => _dataService.GetData().Unlocked[_dataService.GetData().CurrentLocationIndex];
        public void ResetChosenLocation() => _chosenLocationIndex = _locationIds.IndexOf(UnlockedLocationId);
    }
}