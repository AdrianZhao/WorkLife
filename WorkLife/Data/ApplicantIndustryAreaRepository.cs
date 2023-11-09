using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class ApplicantIndustryAreaRepository : IRepository<ApplicantIndustryArea>
    {
        private WorkLifeContext _context;
        public ApplicantIndustryAreaRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public ApplicantIndustryArea Create(ApplicantIndustryArea entity)
        {
            _context.ApplicantIndustryAreas.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(ApplicantIndustryArea entity)
        {
            _context.ApplicantIndustryAreas.Remove(entity);
            _context.SaveChanges();
        }
        public ApplicantIndustryArea Get(int id)
        {
            ApplicantIndustryArea applicantIndustryArea = _context.ApplicantIndustryAreas.Find(id);
            return applicantIndustryArea;
        }
        public ICollection<ApplicantIndustryArea> GetAll()
        {
            ICollection<ApplicantIndustryArea> applicantIndustryAreas = _context.ApplicantIndustryAreas.Include(a => a.Applicant).Include(e => e.IndustryArea).ToList();
            return applicantIndustryAreas;
        }
        public ApplicantIndustryArea Update(ApplicantIndustryArea entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
