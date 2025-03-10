﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        //public virtual ICollection<Place> Places { get; set; } = new List<Place>();
        public Category() { }
        public Category(Models.Category category)
        {
            this.CategoryId = category.CategoryId;
            this.CategoryName = category.CategoryName;
            //this.Places = (ICollection<Place>)category.Places;
        }
    }
}
