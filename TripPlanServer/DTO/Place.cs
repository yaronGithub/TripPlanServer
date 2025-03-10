﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string PlacePicUrl { get; set; } = null!;
        public string PlaceName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string PlaceDescription { get; set; } = null!;
        public double Xcoor { get; set; }
        public double Ycoor { get; set; }
        public string? GooglePlaceId { get; set; }

        public virtual Category? Category { get; set; }
        //public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
        //public virtual ICollection<PlanPlace> PlanPlaces { get; set; } = new List<PlanPlace>();

        public Place() { }
        public Place(Models.Place place)
        {
            this.PlaceId = place.PlaceId;
            this.PlacePicUrl = place.PlacePicUrl;
            this.PlaceName = place.PlaceName;
            this.CategoryId = place.CategoryId;
            this.PlaceDescription = place.PlaceDescription;
            this.Xcoor = place.Xcoor ?? 0.0;
            this.Ycoor = place.Ycoor ?? 0.0;
            this.Category = new Category
            {
                CategoryId = place.Category.CategoryId,
                CategoryName = place.Category.CategoryName
            };
            //this.Pictures = (ICollection<Picture>)place.Pictures;
            //this.PlanPlaces = (ICollection<PlanPlace>)place.PlanPlaces;
        }
    }
}
