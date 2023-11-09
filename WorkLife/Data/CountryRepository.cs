using WorkLife.Models;

namespace WorkLife.Data
{
    public class CountryRepository : IRepository<Country>
    {
        private WorkLifeContext _context;
        public CountryRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public Country Create(Country entity)
        {
            _context.Countries.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Country entity)
        {
            _context.Countries.Remove(entity);
            _context.SaveChanges();
        }
        public Country Get(int id)
        {
            Country country = _context.Countries.Find(id);
            return country;
        }
        public ICollection<Country> GetAll()
        {
            ICollection<Country> countries = _context.Countries.ToList();
            return countries;
        }
        public Country Update(Country entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
