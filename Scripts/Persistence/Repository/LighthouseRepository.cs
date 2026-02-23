using Lighthouse.Dto;
using Lighthouse.Stats;

namespace Persistence.Repository
{
    public class LighthouseRepository : EncryptedOnPremiseRepository<LighthouseDataDto>
    {
        protected override string FilePath => "Lighthouse";
    }
}