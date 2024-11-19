﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripPlanServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TripPlanServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class TripPlanAPIController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private TripPlanDbContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public TripPlanAPIController(TripPlanDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }


        [HttpGet("getAllPlannings")]
        public IActionResult GetAllPlannings([FromBody] string email)
        {
            return Ok("All Plannings");
        }


        [HttpPost("addPlanning")]
        public IActionResult AddPlanning([FromBody] DTO.PlanGroup userPlanDto)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.User? user = context.GetUser(userEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (user == null || (user.IsManager == false && userPlanDto.UserId != user.UserId))
                {
                    return Unauthorized("Non Manager User is trying to add a task for a different user");
                }

                Models.PlanGroup plan = new PlanGroup()
                {
                    UserId = userPlanDto.UserId,
                    GroupName = userPlanDto.GroupName,
                    GroupDescription = userPlanDto.GroupDescription,
                    IsPublished = userPlanDto.IsPublished,
                    StartDate = userPlanDto.StartDate,
                    EndDate = userPlanDto.EndDate,
                    Pictures = (ICollection<Picture>)userPlanDto.Pictures,
                    PlanPlaces = (ICollection<PlanPlace>)userPlanDto.PlanPlaces,
                    Reviews = (ICollection<Review>)userPlanDto.Reviews,
                    //User = userPlanDto.User,
                    Users = (ICollection<User>)userPlanDto.Users,
                    UsersNavigation = (ICollection<User>)userPlanDto.UsersNavigation,
                    PlanId = userPlanDto.PlanId
                };

                context.Entry(plan).State = EntityState.Added;
                context.SaveChanges();

                //Task was added!
                return Ok(new DTO.PlanGroup(plan));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("updatePlanning")]
        public IActionResult UpdatePlanning([FromBody] DTO.PlanGroup userPlanDto)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.User? user = context.GetUser(userEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();
                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (user == null || (user.IsManager == false && userPlanDto.UserId != user.UserId))
                {
                    return Unauthorized("Non Manager User is trying to update a plan for a different user");
                }

                Models.PlanGroup plan = new PlanGroup()
                {
                    UserId = userPlanDto.UserId,
                    GroupName = userPlanDto.GroupName,
                    GroupDescription = userPlanDto.GroupDescription,
                    IsPublished = userPlanDto.IsPublished,
                    StartDate = userPlanDto.StartDate,
                    EndDate = userPlanDto.EndDate,
                    Pictures = (ICollection<Picture>)userPlanDto.Pictures,
                    PlanPlaces = (ICollection<PlanPlace>)userPlanDto.PlanPlaces,
                    Reviews = (ICollection<Review>)userPlanDto.Reviews,
                    //User = userPlanDto.User,
                    Users = (ICollection<User>)userPlanDto.Users,
                    UsersNavigation = (ICollection<User>)userPlanDto.UsersNavigation,
                    PlanId = userPlanDto.PlanId
                };

                context.Entry(plan).State = EntityState.Modified;

                //Now loop through the comments and update / add all of them
                foreach (var review in userPlanDto.Reviews)
                {
                    //check if comment is new or not
                    if (review.ReviewId == 0)
                    {
                        //New comment
                        Models.Review newPlanReview = new Review()
                        {
                            PlanId = plan.PlanId,
                            ReviewDate = review.ReviewDate,
                            ReviewText = review.ReviewText
                        };

                        context.Entry(newPlanReview).State = EntityState.Added;
                    }
                    else
                    {
                        //Update the existing comment
                        Models.Review planReview = new Review()
                        {
                            PlanId = plan.PlanId,
                            ReviewId = review.ReviewId,
                            ReviewDate = review.ReviewDate,
                            ReviewText = review.ReviewText
                        };

                        context.Entry(planReview).State = EntityState.Modified;
                    }

                }
                context.SaveChanges();

                //Plan was updated!
                return Ok(new DTO.PlanGroup(plan));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] TripPlanServer.DTO.LoginInfo loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                TripPlanServer.Models.User? modelsUser = context.GetUser(loginDto.Email);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.Passwd != loginDto.Passwd)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsUser.Email);

                DTO.User dtoUser = new DTO.User(modelsUser);
                dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] TripPlanServer.DTO.User userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Create model user class
                Models.User modelsUser = userDto.GetModels();

                context.Users.Add(modelsUser);
                context.SaveChanges();

                //User was added!
                DTO.User dtoUser = new DTO.User(modelsUser);
                dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("updateUser")]
        public IActionResult UpdateUser([FromBody] DTO.User userDto)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.User? user = context.GetUser(userEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (user == null || (/*user.IsManager == false &&*/ userDto.UserId != user.UserId))
                {
                    return Unauthorized("Non Manager User is trying to update a different user");
                }

                Models.User appUser = userDto.GetModels();

                context.Entry(appUser).State = EntityState.Modified;

                context.SaveChanges();

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("loggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Models.User? user = context.GetUser(userEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (user == null)
            {
                return Unauthorized("User is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{user.UserId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }

            DTO.User dtoUser = new DTO.User(user);
            dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
            return Ok(dtoUser);
        }


        //Helper functions

        //this function gets a file stream and check if it is an image
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

        //this function check which profile image exist and return the virtual path of it.
        //if it does not exist it returns the default profile image virtual path
        private string GetProfileImageVirtualPath(int userId)
        {
            string virtualPath = $"/profileImages/{userId}";
            string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/profileImages/default.png";
                }
            }

            return virtualPath;
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("I am working!");
        }
    }
}
