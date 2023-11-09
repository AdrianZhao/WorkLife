using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WorkLife.Models;

namespace WorkLife.Areas.Identity.Data;

public class WorkLifeUser : IdentityUser
{
    public int? ApplicantId { get; set; }
    public Applicant? Applicant { get; set; }
    public int? EmployerId { get; set; }
    public Employer? Employer { get; set; }
    public string ChoseRole { get; set; }
}