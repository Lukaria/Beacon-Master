using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Lighthouse.Configs;
using Lighthouse.Dto;
using Lighthouse.Stats;
using Lighthouse.Types;
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
            EnrichData();
        }

        public async UniTask SaveAsync()
        {
            await _writeService.CreateAsync(_data);
        }

        public void EnrichData()
        {
            var ids = Enum.GetValues(typeof(LighthouseId)).Cast<LighthouseId>().ToList();
            foreach (var id in ids)
            {
                if (_data.StatLevels.All(x => x.Id != id))
                {
                    _data.StatLevels.Add(new LighthouseStatLevels(id, _repository.configs[id].Stats));
                }
            }
        }
    }
}