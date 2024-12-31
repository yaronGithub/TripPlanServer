using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class PlanPlace
    {
        public int PlaceId { get; set; }

        public int PlanId { get; set; }

        public DateTime? PlaceDate { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

        public virtual Place Place { get; set; } = null!;

        public virtual PlanGroup Plan { get; set; } = null!;

        public PlanPlace(Models.PlanPlace planPlace) 
        {
            this.PlaceId = planPlace.PlaceId;
            this.PlanId = planPlace.PlanId;
            this.PlaceDate = planPlace.PlaceDate;
            this.Pictures = new List<Picture>();
            foreach (var picture in planPlace.Pictures)
            {
                this.Pictures.Add(new Picture(picture));
            }
            this.Place = new Place() 
            {
                PlaceId = this.PlaceId,
                PlacePicUrl = planPlace.Place.PlacePicUrl,
                PlaceName = planPlace.Place.PlaceName,
                CategoryId = planPlace
                .Place
                    .CategoryId,
                PlaceDescription = planPlace.Place.PlaceDescription,
                Xcoor = planPlace.Place.Xcoor,
                Ycoor = planPlace.Place.Ycoor,
                GooglePlaceId = planPlace.Place.GooglePlaceId,
                //Pictures = planPlace.Place.Pictures,
                //PlanPlaces = planPlace.Place.PlanPlaces
            };
            this.Plan = new PlanGroup(planPlace.Plan);
        }
    }
}
