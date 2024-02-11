
namespace PaparaApp.Project.API.Models.Flats
{
    public class FlatRepository(AppDbContext dbContext) : IFlatRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public Flat Add(Flat flat)
        {
            _dbContext.Flats.Add(flat);
            return flat;
        }
        public IEnumerable<Flat> GetFlatsWihtinBlock(string blockNumber) => _dbContext.Flats.Where(f => f.BlockInfo == blockNumber && f.Status == true);

        public void Delete(Guid id)
        {
           var flatToDelete = _dbContext.Flats.FirstOrDefault(f => f.Id == id);
            _dbContext.Flats.Remove(flatToDelete);

        }

        public List<Flat> GetAll() => _dbContext.Flats.ToList();

        public List<Flat> GetAllFlatsWithTenants() => _dbContext.Flats.Where(f => f.Status).ToList();


        public Flat? GetById(Guid id) => _dbContext.Flats.FirstOrDefault(f => f.Id == id);

        public IEnumerable<Guid> GetIdByBlock(string blockNumber) => _dbContext.Flats.Where(f => f.BlockInfo == blockNumber).Select(flat => flat.Id);


        public Flat Update(Flat flat)
        {
            _dbContext.Flats.Update(flat);
             return flat;
        }
    }
}
