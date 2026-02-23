using System;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Lighthouse.Types;
using Persistence.Interfaces;
using Player.Signals;
using Zenject;

namespace Player
{
    public class PlayerDataService : IDataService<PlayerDataDto>
    {
        private PlayerDataDto _playerData;
        private ICreateRepository<PlayerDataDto> _writeService;
        private IReadRepository<PlayerDataDto> _readService;
        
        
        private readonly List<LighthouseId> _lighthouseIds =
            Enum.GetValues(typeof(LighthouseId)).Cast<LighthouseId>().ToList();
        
        private int _chosenLighthouseIndex;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            IReadRepository<PlayerDataDto> readService,
            ICreateRepository<PlayerDataDto> writeService
        )
        {
            _signalBus = signalBus;
            _readService = readService;
            _writeService =  writeService;
        }

        public PlayerDataDto GetData() => _playerData;

        public async UniTask LoadAsync()
        {
            _playerData = await _readService.ReadAsync() ?? new PlayerDataDto();
            ResetChosenLighthouse();
        }

        public async UniTask SaveAsync()
        {
            await _writeService.CreateAsync(_playerData);
        }
        
        public LighthouseId SetPreviousLighthouse()
        {
            _chosenLighthouseIndex--;
            if (_chosenLighthouseIndex < 0) _chosenLighthouseIndex = _lighthouseIds.Count - 1;

            var id = _lighthouseIds[_chosenLighthouseIndex];
            if(_playerData.Unlocked.Contains(id))
            {
                _playerData.CurrentUnlockedIndex = _playerData.Unlocked.IndexOf(id);
            }
            return id;
        }
        
        public LighthouseId SetNextLighthouse()
        {
            _chosenLighthouseIndex++;
            if (_chosenLighthouseIndex >= _lighthouseIds.Count) _chosenLighthouseIndex = 0;
            var id = _lighthouseIds[_chosenLighthouseIndex];
            if(_playerData.Unlocked.Contains(id))
            {
                _playerData.CurrentUnlockedIndex = _playerData.Unlocked.IndexOf(id);
            }
            
            return id;
        }

        public void UnlockLighthouse(LighthouseId id)
        {
            _playerData.Unlocked.Add(id);
            _playerData.CurrentUnlockedIndex = _playerData.Unlocked.IndexOf(id);
        }
        
        public bool IsChosenLighthouseLocked() => !_playerData.Unlocked.Contains(_lighthouseIds[_chosenLighthouseIndex]);

        public bool TrySubtractCash(int price)
        {
            if(_playerData.Cash - price < 0) return false;
            
            _signalBus.AbstractFire<CashUpdatedSignal>();
            _playerData.Cash -= price;
            return true;
        }

        public void AddCash(int price)
        {
            _playerData.Cash += price;
            _signalBus.AbstractFire<CashUpdatedSignal>();
        }
        
        public int GetCash() => _playerData.Cash;
        
        
        public void SetLighthouseId(LighthouseId id)
        {
            if (_playerData.Unlocked.Contains(id))
            {
                _playerData.CurrentUnlockedIndex = _playerData.Unlocked.IndexOf(id);
            }
            else
            {
                Utils.Assertions.Assert("lighthouse id not found");
            }
        }
        
        public LighthouseId UnlockedLighthouseId => _playerData.Unlocked[_playerData.CurrentUnlockedIndex];

        public LighthouseId ChosenLighthouseId => _lighthouseIds[_chosenLighthouseIndex];
        
        public void ResetChosenLighthouse() => _chosenLighthouseIndex = _lighthouseIds.IndexOf(UnlockedLighthouseId);
    }
}