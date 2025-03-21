using System.ComponentModel.DataAnnotations.Schema;

public class SaleRecord
{
    public int Id { get; set; }
    public int GasType { get; set; }
    public double SoldFuel { get; set; }
    public double TotalPrice { get; set; }
    public DateTime SaleDate { get; set; }

    [ForeignKey("Users")]
    public int? UserId { get; set; }
    public UserModel? Users { get; set; } 
}
