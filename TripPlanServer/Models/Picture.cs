using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

public partial class Picture
{
    [Key]
    public int PicId { get; set; }

    public int? PlanId { get; set; }

    public int? PlaceId { get; set; }

    [StringLength(400)]
    public string PicExt { get; set; } = null!;

    [ForeignKey("PlaceId")]
    [InverseProperty("Pictures")]
    public virtual Place? Place { get; set; }

    [ForeignKey("PlanId")]
    [InverseProperty("Pictures")]
    public virtual PlanGroup? Plan { get; set; }

    [ForeignKey("PlanId, PlaceId")]
    [InverseProperty("Pictures")]
    public virtual PlanPlace? PlanPlace { get; set; }

    [InverseProperty("Pic")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
