using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(AutoGenerateField = false)]
    public Guid Id { get; set; }

    public string Title { get; set; }
    public bool IsActive { get; set; } = true;
}