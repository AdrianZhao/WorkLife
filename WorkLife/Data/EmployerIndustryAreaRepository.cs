using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class EmployerIndustryAreaRepository : IRepository<EmployerIndustryArea>
    {
        private WorkLifeContext _context;
        public EmployerIndustryAreaRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public EmployerIndustryArea Create(EmployerIndustryArea entity)
        {
            _context.EmployerIndustryAreas.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(EmployerIndustryArea entity)
        {
            _context.EmployerIndustryAreas.Remove(entity);
            _context.SaveChanges();
        }
        public EmployerIndustryArea Get(int id)
        {
            EmployerIndustryArea employerIndustryArea = _context.EmployerIndustryAreas.Find(id);
            return employerIndustryArea;
        }
        public ICollection<EmployerIndustryArea> GetAll()
        {
            ICollection<EmployerIndustryArea> employerIndustryAreas = _context.EmployerIndustryAreas.Include(a => a.Employer).Include(e => e.IndustryArea).ToList();
            return employerIndustryAreas;
        }
        public EmployerIndustryArea Update(EmployerIndustryArea entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
