using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class Review
    {
        public int ReviewId { get; set; }

        public string Title { get; set; } = null!;

        public int? PlanId { get; set; }

        public int? UserId { get; set; }

        public int? Stars { get; set; }

        public string ReviewText { get; set; } = null!;

        public virtual PlanGroup? Plan { get; set; }

        public virtual User? User { get; set; }

        public Review() { }
    }
}
