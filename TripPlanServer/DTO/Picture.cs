using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class Picture
    {
        public int PicId { get; set; }
        public int? PlanId { get; set; }
        public int? PlaceId { get; set; }
        public string PicExt { get; set; } = null!;
        public virtual Place? Place { get; set; }
        public virtual PlanGroup? Plan { get; set; }
        public virtual PlanPlace? PlanPlace { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public Picture() { }
    }
}
