using GasStationApp;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

public class AddFuelController : Controller
{
    private readonly ApplicationDbContext _context;

    public AddFuelController(ApplicationDbContext context)
    {
        _context = context;
    }

    public double GetOccupancyFromDatabase(int gasType)
    {
        var storage = _context.Storages.FirstOrDefault(s => s.GasType == gasType);
        if (storage != null)
        {
            return (storage.Occupancy / storage.TotalCapacity) * 100;
        }
        return 0; 
    }
    
    [HttpPost]
    public IActionResult AddFuel(int GasType, double AddedFuel)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            ModelState.AddModelError("", "Geçerli bir kullanıcı kimliği alınamadı.");
            return RedirectToAction("AddFuel");
        }

        if (GasType <= 0 || AddedFuel <= 0)
        {
            ModelState.AddModelError("", "Geçerli bir yakıt tipi ve miktar girin.");
            return RedirectToAction("AddFuel");
        }

        var storage = _context.Storages.FirstOrDefault(s => s.GasType == GasType && s.UserId == userId);

        if (storage != null)
        {
            double availableSpace = storage.TotalCapacity - storage.Occupancy;

            if (AddedFuel > availableSpace)
            {
                ModelState.AddModelError("", $"Kapasiteyi aşan bir miktar eklenemez. Mevcut doluluk: {storage.Occupancy} L. Eklenebilir miktar: {availableSpace} L.");
                return RedirectToAction("AddFuel");
            }

            storage.Occupancy += AddedFuel;
        }
        else
        {
            _context.Storages.Add(new StorageInfo
            {
                GasType = GasType,
                Occupancy = AddedFuel,
                TotalCapacity = 10000, 
                UserId = userId 
            });
        }

        _context.SaveChanges();
        return RedirectToAction("AddFuel");
    }

    public StorageInfo GetStorageInfo(int gasType)
    {
        var storageInfo = new StorageInfo
        {
            GasType = gasType,
            Occupancy = GetOccupancyFromDatabase(gasType)
        };
        return storageInfo;
    }

    public StorageInfo UpdateStorageInfo(StorageInfo storageInfo)
    {
        if (storageInfo.AddedFuel > 0)
        {
            AddFuel(storageInfo.GasType, storageInfo.AddedFuel);
        }

        storageInfo.Occupancy = GetOccupancyFromDatabase(storageInfo.GasType);

        return storageInfo;
    }
    
}
