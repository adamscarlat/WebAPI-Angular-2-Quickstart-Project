using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Hero
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HeroId { get; set; }
    
    [Required]
    public string HeroName { get; set; }
}