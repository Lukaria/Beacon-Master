using Location;

namespace Persistence.Repository
{
    public class LocationsRepository : EncryptedOnPremiseRepository<LocationDataDto>
    {
        protected override string FilePath => "Locations";
    }
}