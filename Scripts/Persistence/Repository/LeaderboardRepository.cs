using Leaderboard;

namespace Persistence.Repository
{
    public class LeaderboardRepository : EncryptedOnPremiseRepository<LeaderboardDataDto>
    {
        protected override string FilePath => "Leaderboard";
    }
}