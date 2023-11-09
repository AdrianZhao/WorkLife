using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkLife.Areas.Identity.Data;
using WorkLife.Data;
using WorkLife.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WorkLifeContextConnection") ?? throw new InvalidOperationException("Connection string 'WorkLifeContextConnection' not found.");

builder.Services.AddDbContext<WorkLifeContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<WorkLifeUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WorkLifeContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IRepository<WorkLifeUser>), typeof(WorkLifeUserUserRepository));
builder.Services.AddScoped(typeof(IRepository<Applicant>), typeof(ApplicantRepository));
builder.Services.AddScoped(typeof(IRepository<Employer>), typeof(EmployerRepository));
builder.Services.AddScoped(typeof(IRepository<Country>), typeof(CountryRepository));
builder.Services.AddScoped(typeof(IRepository<Job>), typeof(JobRepository));
builder.Services.AddScoped(typeof(IRepository<IndustryArea>), typeof(IndustryAreaRepository));
builder.Services.AddScoped(typeof(IRepository<ApplicantIndustryArea>), typeof(ApplicantIndustryAreaRepository));
builder.Services.AddScoped(typeof(IRepository<EmployerIndustryArea>), typeof(EmployerIndustryAreaRepository));
builder.Services.AddScoped(typeof(IRepository<JobIndustryArea>), typeof(JobIndustryAreaRepository));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var services = scope.ServiceProvider;
    var context = scope.ServiceProvider.GetRequiredService<WorkLifeContext>();
    //context.Database.EnsureDeleted();
    //context.Database.Migrate();
    await context.Seed(services); // Seed the database
}
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapControllerRoute(name: "default", pattern: "<pattern>");

// include Razor Pages middleware to routing
app.MapRazorPages();

app.Run();