using nyms.resident.server.Models;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ISocialWorkerDataProvider
    {
        SocialWorker GetSocialWorkerByResidentId(int residentId);
    }
}
