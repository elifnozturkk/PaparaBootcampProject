namespace PaparaApp.Project.API.Models.Flats
{
    public interface IFlatRepository
    {
        List<Flat> GetAll();
        List<Flat> GetAllFlatsWithTenants();
        IEnumerable<Guid> GetIdByBlock(string blockNumber);
        IEnumerable<Flat> GetFlatsWihtinBlock(string blockNumber);
        Flat Add(Flat flat);
        Flat Update(Flat flat);
        Flat? GetById(Guid id);
        void Delete(Guid id);

    }
}
