namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IDatabaseSetupProvider
    {
        void SeedDatabase();
        void ClearDatabase();
    }
}
