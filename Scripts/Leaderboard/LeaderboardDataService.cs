using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Persistence.Interfaces;
using Zenject;

namespace Leaderboard
{
    public class LeaderboardDataService : IDataService<LeaderboardDataDto>
    {
        private LeaderboardDataDto _data;
        private IReadRepository<LeaderboardDataDto> _readService;
        private ICreateRepository<LeaderboardDataDto> _writeService;

        [Inject]
        public void Construct(
            IReadRepository<LeaderboardDataDto> readService,
            ICreateRepository<LeaderboardDataDto> writeService)
        {
            _readService = readService;
            _writeService = writeService;
        }

        public LeaderboardDataDto GetData() => _data;

        public async UniTask LoadAsync()
        {
            _data = await _readService.ReadAsync() ?? new LeaderboardDataDto();
        }

        public async UniTask SaveAsync()
        {
            await _writeService.CreateAsync(_data);
        }
    }
}