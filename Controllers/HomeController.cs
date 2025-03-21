using System.Diagnostics;
using System.Text.Json;
using GasStationApp;
using GasStationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly SellController _sellController;  

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SellController sellController)
    {
        _logger = logger;
        _context = context;
        _sellController = sellController;  
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult PriceInfo()
    {
        var fuelPrices = _context.FuelPrices.ToList(); 
        return View(fuelPrices);
    }

    [HttpPost]
    public IActionResult PriceInfo(Dictionary<int, double> Prices)
    {
        if (Prices == null || Prices.Count == 0)
        {
            ModelState.AddModelError("", "Güncellenecek fiyat bulunamadı.");
            return RedirectToAction("PriceInfo");
        }

        foreach (var price in Prices)
        {
            var fuelPrice = _context.FuelPrices.FirstOrDefault(fp => fp.GasType == price.Key);
            if (fuelPrice != null)
            {
                fuelPrice.PricePerLiter = price.Value;
            }
            else
            {
                _context.FuelPrices.Add(new FuelPrice { GasType = price.Key, PricePerLiter = price.Value });
            }
        }

        _context.SaveChanges();
        return RedirectToAction("PriceInfo");
    }

    [HttpGet]
    public IActionResult StorageInfo()
    {
        var storageList = _context.Storages.ToList();
        return View(storageList);
    }

    [HttpGet]
    public IActionResult AddFuel()
    {
        var fuelTypes = _context.Storages.ToList();
        return View(fuelTypes);
    }

    [HttpPost]
    public IActionResult AddFuel(int GasType, double AddedFuel)
    {
        if (GasType <= 0 || AddedFuel <= 0)
        {
            ModelState.AddModelError("", "Geçerli bir yakıt tipi ve miktar girin.");
            return RedirectToAction("AddFuel");
        }

        var storage = _context.Storages.FirstOrDefault(s => s.GasType == GasType);
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
                TotalCapacity = 10000
            });
        }

        _context.SaveChanges();
        _logger.LogInformation($"Yakıt Eklendi: GasType={GasType}, AddedFuel={AddedFuel}");
        return RedirectToAction("AddFuel");
    }

    [HttpGet]
    public IActionResult SellInfo()
    {
        var storageList = _context.Storages.ToList();

        var soldFuels = _context.SaleRecords.ToList();  
        ViewData["SoldFuels"] = soldFuels;

        var fuelPrices = _context.FuelPrices
            .AsNoTracking()
            .ToDictionary(f => f.GasType, f => f.PricePerLiter);

        ViewData["FuelPricesJson"] = JsonSerializer.Serialize(fuelPrices);

        return View(storageList);
    }

    [HttpPost]
    public IActionResult SellInfo(int GasType, double SelledFuel)
    {
        var storage = _context.Storages.FirstOrDefault(s => s.GasType == GasType);

        if (storage == null || storage.Occupancy <= 0)
        {
            ViewData["WarningMessage"] = " Lütfen geçerli bir yakıt miktarı girin.";
            return View(_context.Storages.ToList());
        }

        if (SelledFuel > storage.Occupancy)
        {
            ViewData["WarningMessage"] = $" Yetersiz yakıt! Mevcut: {storage.Occupancy} L";
            return View(_context.Storages.ToList());
        }

        try
        {
            _sellController.SellFuel(GasType, SelledFuel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return RedirectToAction("SellInfo");
    }

}
