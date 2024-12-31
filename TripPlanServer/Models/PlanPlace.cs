using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

[PrimaryKey("PlanId", "PlaceId")]
[Table("PlanPlace")]
public partial class PlanPlace
{
    [Key]
    public int PlaceId { get; set; }

    [Key]
    public int PlanId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlaceDate { get; set; }

    [InverseProperty("PlanPlace")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    [ForeignKey("PlaceId")]
    [InverseProperty("PlanPlaces")]
    public virtual Place Place { get; set; } = null!;

    [ForeignKey("PlanId")]
    [InverseProperty("PlanPlaces")]
    public virtual PlanGroup Plan { get; set; } = null!;
}
