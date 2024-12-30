using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


    public List<PlanPlace>? GetAllPlacesByEmail(string email, int planId)
    {
        return this.PlanPlaces
        .Where(pp => pp.PlanId == planId &&
                     (pp.Plan.User != null && pp.Plan.User.Email == email || pp.Plan.Users.Any(u => u.Email == email)))
        .ToList();
    }

    public List<PlanPlace>? GetAllPlacesByEmailAndDateAndPlanId(string email, string dayDate, int planId)
    {
        email = "a"; dayDate = "d"; planId = 222;
        /*return this.PlanPlaces
        .Where(pp => pp.PlanId == planId &&
                     pp.PlaceDate.HasValue &&
                     pp.PlaceDate.Value.ToString("MM/dd/yyyy") == dayDate &&
                     (pp.Plan.User.Email == email || pp.Plan.Users.Any(u => u.Email == email)))
        .ToList();*/
        // return this.PlanPlaces.ToList();
        var result = this.PlanPlaces.ToList();

        foreach (var planPlace in result)
        {
            //planPlace.Plan = this.PlanGroups.FirstOrDefault(pg => pg.PlanId == planPlace.PlanId);
            //planPlace.Place = this.Places.FirstOrDefault(p => p.PlaceId == planPlace.PlaceId);
            planPlace.Plan = new PlanGroup();
            planPlace.Place = new Place();

            if (planPlace.Plan == null || planPlace.Place == null)
            {
                throw new InvalidOperationException("PlanPlace contains null Plan or Place.");
            }
        }

        return result;
    }
}
