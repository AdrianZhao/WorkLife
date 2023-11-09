using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class JobIndustryAreaRepository : IRepository<JobIndustryArea>
    {
        private WorkLifeContext _context;
        public JobIndustryAreaRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public JobIndustryArea Create(JobIndustryArea entity)
        {
            _context.JobIndustryAreas.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(JobIndustryArea entity)
        {
            _context.JobIndustryAreas.Remove(entity);
            _context.SaveChanges();
        }
        public JobIndustryArea Get(int id)
        {
            JobIndustryArea jobIndustryArea = _context.JobIndustryAreas.Find(id);
            return jobIndustryArea;
        }
        public ICollection<JobIndustryArea> GetAll()
        {
            ICollection<JobIndustryArea> jobIndustryAreas = _context.JobIndustryAreas.Include(a => a.Job).Include(e => e.IndustryArea).ToList();
            return jobIndustryAreas;
        }
        public JobIndustryArea Update(JobIndustryArea entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
