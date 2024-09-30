using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

[Index("Email", Name = "UQ__Users__A9D10534626DD14F", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Passwd { get; set; } = null!;

    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

    public int? PicId { get; set; }

    [ForeignKey("PicId")]
    [InverseProperty("Users")]
    public virtual Picture? Pic { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<PlanGroup> PlanGroups { get; set; } = new List<PlanGroup>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<PlanGroup> Plans { get; set; } = new List<PlanGroup>();

    [ForeignKey("UserId")]
    [InverseProperty("UsersNavigation")]
    public virtual ICollection<PlanGroup> PlansNavigation { get; set; } = new List<PlanGroup>();
}
