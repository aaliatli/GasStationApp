using GasStationApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class SellController : Controller
{
    private readonly ApplicationDbContext _context;

    public SellController(ApplicationDbContext context)
    {
        _context = context;
    }

    public double GetPricePerLiter(int gasType)
    {
        var fuelPrice = _context.FuelPrices
            .AsNoTracking() 
            .FirstOrDefault(f => f.GasType == gasType);

        if (fuelPrice != null)
        {
            return fuelPrice.PricePerLiter;
        }

        throw new InvalidOperationException("Yakıt fiyatı bulunamadı.");
    }

    public IActionResult SellFuel(int gasType, double soldFuel)
    {
        try
        {
            var storage = _context.Storages.FirstOrDefault(s => s.GasType == gasType);
            if (storage == null)
            {
                return BadRequest("Yakıt türü bulunamadı!");
            }

            if (storage.Occupancy < soldFuel)
            {
                return BadRequest("Yetersiz yakıt! Satış için yeterli yakıt bulunmamaktadır.");
            }

            var fuelPrice = _context.FuelPrices.AsNoTracking().FirstOrDefault(f => f.GasType == gasType);
            if (fuelPrice == null)
            {
                return BadRequest("Yakıt fiyatı bulunamadı.");
            }

            double totalPrice = fuelPrice.PricePerLiter * soldFuel;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    storage.Occupancy -= soldFuel;

                    var saleRecord = new SaleRecord
                    {
                        GasType = gasType,
                        SoldFuel = soldFuel,
                        TotalPrice = totalPrice,
                        SaleDate = DateTime.Now
                    };

                    _context.SaleRecords.Add(saleRecord);
                    _context.SaveChanges();

                    transaction.Commit(); 
                }
                catch
                {
                    transaction.Rollback(); 
                    return StatusCode(500, "Satış işlemi sırasında bir hata oluştu!");
                }
            }

            return Ok($"Yakıt satışı başarıyla gerçekleştirildi. Toplam Fiyat: {totalPrice} TL");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Beklenmeyen bir hata oluştu: {ex.Message}");
        }
    }
}
