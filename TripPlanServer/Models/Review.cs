using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

public partial class Review
{
    [Key]
    public int ReviewId { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    public int? PlanId { get; set; }

    public int? UserId { get; set; }

    public int? Stars { get; set; }

    [StringLength(100)]
    public string ReviewText { get; set; } = null!;

    [ForeignKey("PlanId")]
    [InverseProperty("Reviews")]
    public virtual PlanGroup? Plan { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Reviews")]
    public virtual User? User { get; set; }
}
