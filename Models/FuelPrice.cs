using System.ComponentModel.DataAnnotations;

public class FuelPrice{
    [Key]
    public int Id {get; set;}
    public int GasType {get; set;}
    public double PricePerLiter {get; set;}
}
