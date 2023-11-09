using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class EmployerRepository : IRepository<Employer>
    {
        private WorkLifeContext _context;
        public EmployerRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public Employer Create(Employer entity)
        {
            _context.Employers.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Employer entity)
        {
            _context.Employers.Remove(entity);
            _context.SaveChanges();
        }
        public Employer Get(int id)
        {
            Employer employer = _context.Employers.Find(id);
            return employer;
        }
        public ICollection<Employer> GetAll()
        {
            ICollection<Employer> employers = _context.Employers.Include(e => e.IndustryAreas).ToList();
            return employers;
        }
        public Employer Update(Employer entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
