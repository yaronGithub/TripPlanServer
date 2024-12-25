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

    public List<PlanGroup>? GetAllPlanningsByEmail(string email)
    {
        return this.PlanGroups.Where(pg => pg.Users.Any(u => u.Email == email) || email == pg.User.Email).ToList();
    }

    public List<PlanGroup>? GetAllPublishedPlannings()
    {
        return this.PlanGroups.Where(pg => pg.IsPublished == true).ToList();
    }

    public int GetFreePlanId()
    {
        return this.PlanGroups.Count()+1;
    }


    public List<PlanPlace>? GetAllPlacesByEmail(string email)
    {
        //return this.PlanPlaces.Where(pp => pp.Plan.User.Email == email).ToList();
        return null;
    }

    public List<PlanPlace>? GetAllPlacesByEmailAndDate(string email, string dayDate)
    {
        //throw new NotImplementedException();
        return null;
    }
}
