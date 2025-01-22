using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

public partial class Place
{
    [Key]
    public int PlaceId { get; set; }

    [Column("PlacePicURL")]
    [StringLength(400)]
    public string PlacePicUrl { get; set; } = null!;

    [StringLength(50)]
    public string PlaceName { get; set; } = null!;

    public int? CategoryId { get; set; }

    [StringLength(300)]
    public string PlaceDescription { get; set; } = null!;

    public double? Xcoor { get; set; }

    public double? Ycoor { get; set; }

    [StringLength(100)]
    public string? GooglePlaceId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Places")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Place")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    [InverseProperty("Place")]
    public virtual ICollection<PlanPlace> PlanPlaces { get; set; } = new List<PlanPlace>();
}
