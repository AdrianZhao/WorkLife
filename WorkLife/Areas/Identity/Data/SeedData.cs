using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkLife.Data;
using WorkLife.Models;

namespace WorkLife.Areas.Identity.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            WorkLifeContext context = new WorkLifeContext(serviceProvider.GetRequiredService<DbContextOptions<WorkLifeContext>>());
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Employer", "Applicant" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            /*
            if (!context.IndustryAreas.Any())
            {
                context.IndustryAreas.AddRange(
                    new IndustryArea { Title = "Software Development" },
                    new IndustryArea { Title = "Web Development" },
                    new IndustryArea { Title = "Mobile App Development" },
                    new IndustryArea { Title = "Database Administration" },
                    new IndustryArea { Title = "Cloud Computing" },
                    new IndustryArea { Title = "DevOps" },
                    new IndustryArea { Title = "Artificial Intelligence (AI)" },
                    new IndustryArea { Title = "Machine Learning" },
                    new IndustryArea { Title = "Data Science" },
                    new IndustryArea { Title = "Cybersecurity" },
                    new IndustryArea { Title = "Network Administration" },
                    new IndustryArea { Title = "IT Support and Helpdesk" },
                    new IndustryArea { Title = "Quality Assurance and Testing" },
                    new IndustryArea { Title = "IT Project Management" },
                    new IndustryArea { Title = "Business Analysis" },
                    new IndustryArea { Title = "IT Consulting" },
                    new IndustryArea { Title = "Game Development" },
                    new IndustryArea { Title = "Virtual Reality (VR) and Augmented Reality (AR)" },
                    new IndustryArea { Title = "Data Analytics" },
                    new IndustryArea { Title = "UI/UX Design" },
                    new IndustryArea { Title = "Embedded Systems" },
                    new IndustryArea { Title = "Robotics" },
                    new IndustryArea { Title = "Internet of Things (IoT)" },
                    new IndustryArea { Title = "Blockchain Development" },
                    new IndustryArea { Title = "Computer Graphics and Animation" }
                    );
            }
            */
            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                    new Country { Name = "Canada" },
                    new Country { Name = "United States" },
                    new Country { Name = "Other Country" }
                );
            }
            context.SaveChanges();
        }
    }
}

