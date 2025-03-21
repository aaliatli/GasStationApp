using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserModel
{
    [Key]
    [Column("User_ID")]
    public int Id { get; set; }

    [Required]
    public required string UserNameLastName { get; set; }
    [Required]
    public string? UserPhoneNumber { get; set; }

    [Required, EmailAddress]
    public string? UserEmail { get; set; }

    [Required]
    public string? UserPassword { get; set; }

    [Required]
    public int StationCode { get; set; }

    public string? StationName { get; set; }

    public string? CompanyTaxNumber { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    [Required]
    public ICollection<StorageInfo>? Storages { get; set; }
}
