using Common;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Lighthouse.Configs;
using Lighthouse.Dto;
using Persistence.Interfaces;
using Zenject;

namespace Lighthouse
{
    public class LighthouseDataService : IDataService<LighthouseDataDto>
    {
        private IReadRepository<LighthouseDataDto> _readService;
        private ICreateRepository<LighthouseDataDto> _writeService;

        private LighthouseDataDto _data;
        private LighthouseRepositoryData _repository;

        [Inject]
        public void Construct(
            LighthouseRepositoryData repositoryData,
            IReadRepository<LighthouseDataDto> readService,
            ICreateRepository<LighthouseDataDto> writeService)
        {
            _repository = repositoryData;
            _readService = readService;
            _writeService = writeService;
        }

        public LighthouseDataDto GetData() => _data;

        public async UniTask LoadAsync()
        {
            _data = await _readService.ReadAsync() ?? new LighthouseDataDto(_repository);
        }

        public async UniTask SaveAsync()
        {
            await _writeService.CreateAsync(_data);
        }
    }
}