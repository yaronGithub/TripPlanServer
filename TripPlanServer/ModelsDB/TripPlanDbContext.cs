using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

public partial class TripPlanDbContext : DbContext
{
    public User? GetUser(string email)
    {
        return this.Users.Where(u => u.Email == email).FirstOrDefault();
    }

    public List<PlanGroup> GetAllPlanningsByEmail(string email)
    {
        return this.PlanGroups.Where(pg => pg.Users.Any(u => u.Email == email)).ToList();
    }
}
