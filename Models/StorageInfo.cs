using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class StorageInfo
{
    [Key]
    public int Id { get; set; }

    public int GasType { get; set; }

    public double Occupancy { get; set; }

    public double TotalCapacity { get; set; } = 10000;

    public double AddedFuel { get; set; }

    public int UserId { get; set; }

}
