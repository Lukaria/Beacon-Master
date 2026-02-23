using Player;

namespace Persistence.Repository
{
    public class PlayerDataRepository : EncryptedOnPremiseRepository<PlayerDataDto>
    {
        protected override string FilePath => "PlayerData";
    }
}