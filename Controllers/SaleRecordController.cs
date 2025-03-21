using Microsoft.AspNetCore.Mvc;

public class SaleRecordController : Controller
{
    private readonly SaleRecordService _saleRecordService;

    public SaleRecordController(SaleRecordService saleRecordService)
    {
        _saleRecordService = saleRecordService;
    }

    [HttpPost("fix-invalid-user-ids")]
    public IActionResult FixInvalidUserIds()
    {
        try
        {
            _saleRecordService.FixInvalidUserIds();
            return Ok("Geçersiz UserId'ler düzeltildi.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Bir hata oluştu: {ex.Message}");
        }
    }

    [HttpPost("remove-invalid-sale-records")]
    public IActionResult RemoveInvalidSaleRecords()
    {
        try
        {
            _saleRecordService.RemoveInvalidSaleRecords();
            return Ok("Geçersiz kayıtlar silindi.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Bir hata oluştu: {ex.Message}");
        }
    }
}

