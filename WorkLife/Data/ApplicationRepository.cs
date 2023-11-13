using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class ApplicationRepository : IRepository<Application>
    {
        private WorkLifeContext _context;
        public ApplicationRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public Application Create(Application entity)
        {
            _context.Applications.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Application entity)
        {
            _context.Applications.Remove(entity);
            _context.SaveChanges();
        }
        public Application Get(int id)
        {
            Application application = _context.Applications.Find(id);
            return application;
        }
        public ICollection<Application> GetAll()
        {
            ICollection<Application> applications = _context.Applications.Include(a => a.Job).Include(a => a.Applicant).ToList();
            return applications;
        }
        public Application Update(Application entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
