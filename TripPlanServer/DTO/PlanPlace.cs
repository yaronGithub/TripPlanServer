using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class PlanPlace
    {
        public int PlaceId { get; set; }

        public int PlanId { get; set; }

        public DateOnly? PlaceDate { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

        public virtual Place Place { get; set; } = null!;

        public virtual PlanGroup Plan { get; set; } = null!;

        public PlanPlace() { }
    }
}
