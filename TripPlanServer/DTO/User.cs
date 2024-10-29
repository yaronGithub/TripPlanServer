using TripPlanServer.Models;

namespace TripPlanServer.DTO
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Passwd { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int PicId { get; set; }

        public virtual ICollection<PlanGroup> PlanGroups { get; set; } = new List<PlanGroup>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<PlanGroup> Plans { get; set; } = new List<PlanGroup>();

        public User() { }
        public User(Models.User modelUser)
        {
            this.UserId = modelUser.UserId;
            this.FirstName = modelUser.FirstName;
            this.LastName = modelUser.LastName;
            this.Email = modelUser.Email;
            this.Passwd = modelUser.Passwd;
            this.PlanGroups = new List<PlanGroup>();
            this.Reviews = new List<Review>();
            this.Plans = new List<PlanGroup>();
            foreach (var planGroup in modelUser.PlanGroups)
            {
                this.PlanGroups.Add(planGroup);
            }
            foreach (var review in modelUser.Reviews)
            {
                this.Reviews.Add(review);
            }
            foreach (var plan in modelUser.Plans)
            {
                this.Plans.Add(plan);
            }
        }

        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {
                UserId = this.UserId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Passwd = this.Passwd,
                PhoneNumber = this.PhoneNumber,
                PicId = this.PicId,
            };

            return modelsUser;
        }
    }
}
