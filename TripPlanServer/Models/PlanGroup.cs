using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

[Table("PlanGroup")]
public partial class PlanGroup
{
    [Key]
    public int PlanId { get; set; }

    [StringLength(50)]
    public string GroupName { get; set; } = null!;

    public int? UserId { get; set; }

    public bool IsPublished { get; set; }

    [StringLength(100)]
    public string GroupDescription { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [InverseProperty("Plan")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    [InverseProperty("Plan")]
    public virtual ICollection<PlanPlace> PlanPlaces { get; set; } = new List<PlanPlace>();

    [InverseProperty("Plan")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("UserId")]
    [InverseProperty("PlanGroups")]
    public virtual User? User { get; set; }

    [ForeignKey("PlanId")]
    [InverseProperty("Plans")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [ForeignKey("PlanId")]
    [InverseProperty("PlansNavigation")]
    public virtual ICollection<User> UsersNavigation { get; set; } = new List<User>();
}
