using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TokenStore
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public bool IsValid { get; set; }

    [Required]
    public DateTime ExpirationDate { get; set; }
}