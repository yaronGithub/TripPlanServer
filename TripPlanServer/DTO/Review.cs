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
        public DateOnly? ReviewDate { get; set; }

        public virtual PlanGroup? Plan { get; set; }

        public virtual User? User { get; set; }

        public Review() { }
        public Review(Models.Review review) 
        {
            this.ReviewId = review.ReviewId;
            this.Title = review.Title;
            this.PlanId = review.PlanId;
            this.UserId = review.UserId;
            this.Stars = review.Stars;
            this.ReviewText = review.ReviewText;
            //this.Plan = review.Plan;
            //this.User = review.User;
        }
    }
}
