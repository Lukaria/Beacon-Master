using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Persistence.Interfaces;
using Zenject;

namespace Location
{
    public class LocationDataService : IDataService<LocationDataDto>
    {
        private LocationDataDto _dto;
        private IReadRepository<LocationDataDto> _readService;
        private ICreateRepository<LocationDataDto> _writeService;


        [Inject]
        public void Construct(
            IReadRepository<LocationDataDto> readService,
            ICreateRepository<LocationDataDto> writeService)
        {
            _readService = readService;
            _writeService = writeService;
        }

        public LocationDataDto GetData() =>  _dto;

        public async UniTask LoadAsync()
        {
            _dto = await _readService.ReadAsync() ?? new LocationDataDto();
        }

        public async UniTask SaveAsync()
        {
            await _writeService.CreateAsync(_dto);
        }
    }
}