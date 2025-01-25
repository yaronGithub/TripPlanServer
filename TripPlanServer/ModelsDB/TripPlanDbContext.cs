using System;
using System.Collections.Generic;
using System.Globalization;
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
        return this.PlanGroups.Count() + 1;
    }

    public int GetFreePlaceId()
    {
        return this.Places.Count() + 1;
    }

    public List<PlanPlace>? GetAllPlacesByEmail(string email, int planId)
    {
        return this.PlanPlaces
        .Where(pp => pp.PlanId == planId &&
                     (pp.Plan.User != null && pp.Plan.User.Email == email || pp.Plan.Users.Any(u => u.Email == email)))
        .ToList();
    }

    public List<PlanPlace>? GetAllPlacesByEmailAndDateAndPlanId(string dayDate, int planId)
    {
        //DateTime day = DateTime.Parse(dayDate);
        DateTime day = DateTime.ParseExact(dayDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        return this.PlanPlaces
        .Where(pp => pp.PlanId == planId &&
                     pp.PlaceDate.HasValue &&
                     pp.PlaceDate.Value.Date.Equals(day.Date))
        .Include(pp=>pp.Place)
        .ToList();
        // return this.PlanPlaces.ToList();
        //var result = this.PlanPlaces.ToList();

        //foreach (var planPlace in result)
        //{
        //    //planPlace.Plan = this.PlanGroups.FirstOrDefault(pg => pg.PlanId == planPlace.PlanId);
        //    //planPlace.Place = this.Places.FirstOrDefault(p => p.PlaceId == planPlace.PlaceId);
        //    planPlace.Plan = new PlanGroup();
        //    planPlace.Place = new Place()
        //    {
        //        PlaceName = this.Places.FirstOrDefault(p => p.PlaceId == planPlace.PlaceId).PlaceName
        //    };

        //    if (planPlace.Plan == null || planPlace.Place == null)
        //    {
        //        throw new InvalidOperationException("PlanPlace contains null Plan or Place.");
        //    }
        //}

        //return result;
    }
}
