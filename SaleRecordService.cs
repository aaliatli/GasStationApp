using GasStationApp;

public class SaleRecordService
{
    private readonly ApplicationDbContext _context;

    public SaleRecordService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void FixInvalidUserIds()
    {
        var invalidSaleRecords = _context.SaleRecords
            .Where(sr => !_context.Users.Any(u => u.Id == sr.UserId))
            .ToList();

        foreach (var record in invalidSaleRecords)
        {
            record.UserId = 0; 
        }

        _context.SaveChanges();
    }

    public void RemoveInvalidSaleRecords()
    {
        var invalidSaleRecords = _context.SaleRecords
            .Where(sr => !_context.Users.Any(u => u.Id == sr.UserId))
            .ToList();

        _context.SaleRecords.RemoveRange(invalidSaleRecords);
        _context.SaveChanges();
    }
}
