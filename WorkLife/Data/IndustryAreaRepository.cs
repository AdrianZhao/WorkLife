using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class IndustryAreaRepository : IRepository<IndustryArea>
    {
        private WorkLifeContext _context;
        public IndustryAreaRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public IndustryArea Create(IndustryArea entity)
        {
            _context.IndustryAreas.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(IndustryArea entity)
        {
            _context.IndustryAreas.Remove(entity);
            _context.SaveChanges();
        }
        public IndustryArea Get(int id)
        {
            IndustryArea industryArea = _context.IndustryAreas.Find(id);
            return industryArea;
        }
        public ICollection<IndustryArea> GetAll()
        {
            ICollection<IndustryArea> industryAreas = _context.IndustryAreas.Include(i => i.ApplicantsIndustryAreas).Include(i => i.EmployersIndustryAreas).Include(i => i.JobIndustryAreas).ToList();
            return industryAreas;
        }
        public IndustryArea Update(IndustryArea entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
