using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class PlanGroup
    {
        public int PlanId { get; set; }
        public string GroupName { get; set; } = null!;
        public int? UserId { get; set; }
        public bool IsPublished { get; set; }
        public string GroupDescription { get; set; } = null!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
        public virtual ICollection<PlanPlace> PlanPlaces { get; set; } = new List<PlanPlace>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual User? User { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public virtual ICollection<User> UsersNavigation { get; set; } = new List<User>();

        public PlanGroup() { }
        public PlanGroup(Models.PlanGroup planGroup) 
        {
            this.PlanId = planGroup.PlanId;
            this.GroupName = planGroup.GroupName;
            this.UserId = planGroup.UserId;
            this.IsPublished = planGroup.IsPublished;
            this.GroupDescription = planGroup.GroupDescription;
            this.StartDate = planGroup.StartDate;
            this.EndDate = planGroup.EndDate;
            this.Pictures = (ICollection<Picture>)planGroup.Pictures;
            this.PlanPlaces = (ICollection<PlanPlace>)planGroup.PlanPlaces;
            this.Reviews = (ICollection<Review>)planGroup.Reviews;
            //this.User = planGroup.User;
            this.Users = (ICollection<User>)planGroup.Users;
            this.UsersNavigation = (ICollection<User>)planGroup.UsersNavigation;
        }
    }
}
