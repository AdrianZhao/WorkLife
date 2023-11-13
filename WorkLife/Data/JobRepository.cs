using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class JobRepository : IRepository<Job>
    {
        private WorkLifeContext _context;
        public JobRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public Job Create(Job entity)
        {
            _context.Jobs.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Job entity)
        {
            _context.Jobs.Remove(entity);
            _context.SaveChanges();
        }
        public Job Get(int id)
        {
            Job job = _context.Jobs.Find(id);
            return job;
        }
        public ICollection<Job> GetAll()
        {
            ICollection<Job> jobs = _context.Jobs.Include(j => j.IndustryAreas).ThenInclude(ia => ia.IndustryArea).Include(j => j.Applications).ToList();
            return jobs;
        }
        public Job Update(Job entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
