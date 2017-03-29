using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TokenStore
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    public string Token { get; set; }

    [Required]
    public bool IsValid { get; set; }

    //TODO: Add expiration date column based on token expiration
}