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

    public List<User> GetAllUsers()
    {
        return this.Users.ToList();
    }

    public User? GetUserById(int userId)
    {
        return this.Users.Where(u => u.UserId == userId).FirstOrDefault();
    }

    public int GetCategoryId(string categoryName)
    {
        foreach (var category in this.Categories)
        {
            if (category.CategoryName == categoryName)
            {
                return category.CategoryId;
            }
        }
        return this.Categories.Count() + 1;
    }

    public bool CategoryExists(string categoryName)
    {
        return this.Categories.Any(c => c.CategoryName == categoryName);
    }

    public Category GetCategoryByName(string categoryName)
    {
        return this.Categories.Where(c => c.CategoryName == categoryName).FirstOrDefault();
    }

    public List<PlanGroup>? GetAllPlanningsByEmail(string email)
    {
        return this.PlanGroups
                       .Where(pg => pg.UsersNavigation.Any(u => u.Email == email) || email == pg.User.Email)
                       .Include(pg => pg.UsersNavigation) // Include the Users collection
                       .ToList();
    }

    public List<PlanGroup>? GetAllPublishedPlannings()
    {
        return this.PlanGroups.Where(pg => pg.IsPublished == true).Include(pg => pg.Reviews).ToList();
    }

    public int GetFreePlanId()
    {
        return this.PlanGroups.Count() + 1;
    }

    public int GetFreePlaceId()
    {
        return this.Places.Count() + 1;
    }

    public Place? GetGooglePlaceById(string googlePlaceId)
    {
        return this.Places.Where(p => p.GooglePlaceId == googlePlaceId).FirstOrDefault();
    }

    public bool PlaceExists(string googlePlaceId)
    {
        return this.Places.Any(p => p.GooglePlaceId == googlePlaceId);
    }

    public PlanPlace ChangePlanPlace(int placeId, Place place)
    {
        // Retrieve the PlanPlace by placeId
        var planPlace = this.PlanPlaces.Include(pp => pp.Place).FirstOrDefault(pp => pp.PlaceId == placeId);

        if (planPlace == null)
        {
            // If no PlanPlace is found, return null or handle accordingly
            return null;
        }

        // Update the Place property of the PlanPlace
        planPlace.Place = place;

        // Save changes to the database
        this.SaveChanges();

        // Return the updated PlanPlace
        return planPlace;
    }

    public List<PlanPlace>? GetAllPlacesByEmail(string email, int planId)
    {
        return this.PlanPlaces
        .Where(pp => pp.PlanId == planId &&
                     (pp.Plan.User != null && pp.Plan.User.Email == email || pp.Plan.Users.Any(u => u.Email == email)))
        .Include(pp => pp.Place.Category)
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
        .Include(pp=>pp.Place.Category)
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
