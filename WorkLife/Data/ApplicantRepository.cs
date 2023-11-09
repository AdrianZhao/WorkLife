using Microsoft.EntityFrameworkCore;
using WorkLife.Models;

namespace WorkLife.Data
{
    public class ApplicantRepository : IRepository<Applicant>
    {
        private WorkLifeContext _context;
        public ApplicantRepository(WorkLifeContext context)
        {
            _context = context;
        }
        public Applicant Create(Applicant entity)
        {
            _context.Applicants.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(Applicant entity)
        {
            _context.Applicants.Remove(entity);
            _context.SaveChanges();
        }
        public Applicant Get(int id)
        {
            Applicant applicant = _context.Applicants.Find(id);
            return applicant;
        }
        public ICollection<Applicant> GetAll()
        {
            ICollection<Applicant> applicants = _context.Applicants.Include(a => a.IndustryAreas).ToList();
            return applicants;
        }
        public Applicant Update(Applicant entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
