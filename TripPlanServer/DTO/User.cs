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
        public int? PicId { get; set; }


        public User() { }
        public User(Models.User modelUser)
        {
            this.UserId = modelUser.UserId;
            this.FirstName = modelUser.FirstName;
            this.LastName = modelUser.LastName;
            this.Email = modelUser.Email;
            this.Passwd = modelUser.Passwd;
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
