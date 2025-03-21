using System.ComponentModel.DataAnnotations;

namespace GasStationApp{
    public class RegisterViewModel{

        [Key]
        public int RId { get; set; }
        [Required]
        public required String UserNameLastName {get; set;}
        [Required, Phone]
        public String? UserPhoneNumber {get; set;}
        [Required, EmailAddress]
        public String? UserEmail {get; set;}
        [Required]
        public required String UserPassword {get; set;}
        [Required]
        public int StationCode {get; set;}
        [Required]
        public String? StationName {get; set;}
        [Required]
        public String? CompanyTaxNumber {get; set;}
        
        public String? City {get; set;}
        
        public String? Province {get; set;}

    }
}
