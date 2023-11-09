using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkLife.Areas.Identity.Data;
using WorkLife.Models;

namespace WorkLife.Data;

public class WorkLifeContext : IdentityDbContext<WorkLifeUser>
{
    public WorkLifeContext(DbContextOptions<WorkLifeContext> options)
        : base(options)
    {
    }
    public DbSet<Applicant> Applicants { get; set; } = default!;
    public DbSet<Employer> Employers { get; set; } = default!;
    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Job> Jobs { get; set; } = default!;
    public DbSet<IndustryArea> IndustryAreas { get; set; } = default!;
    public DbSet<ApplicantIndustryArea> ApplicantIndustryAreas { get; set; } = default!;
    public DbSet<EmployerIndustryArea> EmployerIndustryAreas { get; set; } = default!;
    public DbSet<JobIndustryArea> JobIndustryAreas { get; set; } = default!;
    public async Task Seed(IServiceProvider serviceProvider)
    {
        await SeedData.Initialize(serviceProvider);
    }
}
