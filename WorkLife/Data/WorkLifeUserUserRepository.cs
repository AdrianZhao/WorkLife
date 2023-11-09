using Microsoft.EntityFrameworkCore;
using WorkLife.Areas.Identity.Data;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class WorkLifeUserUserRepository : IRepository<WorkLifeUser>
    {
        private WorkLifeContext _context;
        public WorkLifeUserUserRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public WorkLifeUser Create(WorkLifeUser entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(WorkLifeUser entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }
        public WorkLifeUser Get(int id)
        {
            WorkLifeUser user = _context.Users.Find(id);
            return user;
        }
        public ICollection<WorkLifeUser> GetAll()
        {
            ICollection<WorkLifeUser> users = _context.Users.Include(u => u.Applicant).ThenInclude(a => a.IndustryAreas).ThenInclude(i => i.IndustryArea).Include(u => u.Employer).ThenInclude(e => e.IndustryAreas).ThenInclude(i => i.IndustryArea).ToList();
            return users;
        }
        public WorkLifeUser Update(WorkLifeUser entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
