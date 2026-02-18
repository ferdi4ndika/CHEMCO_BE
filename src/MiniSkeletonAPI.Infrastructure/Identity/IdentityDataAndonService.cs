using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using MiniSkeletonAPI.Application.Identity.DataAndons.Queries.GetDataAndonsWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType; 

namespace MiniSkeletonAPI.Infrastructure.Identity;

public class IdentityDataAndonService : IIdentityDataAndonService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;



    public IdentityDataAndonService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
      
    }


 



    public async Task<Result> UpdateAllDataAndonAsync()
    {
        DateTime now = DateTime.UtcNow.Date;
        DateTime thirtySecondsAgo = now.AddSeconds(-1);

        var data = await _context.DataAndons
            .Where(x => x.CreatedAt < thirtySecondsAgo && x.Status == 0)
            .ToListAsync();

        foreach (var item in data)
        {
            item.Status = 1;
            // item.UpdatedAt = DateTime.UtcNow;
        }

        //await _context.SaveChangesAsync();
        return Result.Success();
    }


    public async Task<Result> UpdateDetail(CancellationToken token)
    {
        //var dataAndon = _context.DataAndons.Where(a=> a.Antrian == "prosess").OrderBy(a=> a.CreatedAt).FirstOrDefault();
        var batasWaktu = DateTime.UtcNow.AddHours(+2);

        var dataAndon = _context.DataAndons
            .Where(a => a.Antrian == "prosess" && a.CreatedAt >= batasWaktu)
            .OrderBy(a => a.CreatedAt)
            .FirstOrDefault();

        if (dataAndon != null)

        {
            if (dataAndon.StarProsess == null)
            {
                dataAndon.StarProsess = DateTime.UtcNow.AddHours(0);
                _context.DataAndons.Update(dataAndon);

            }
            var dataDetail= _context.DataAndonDetails.Where(a => a.IdAndon == dataAndon.Id && a.CountNumber == 0).FirstOrDefault();
            var dataCount = _context.DataCounts.FirstOrDefault()?.CountNumber;
            if (dataDetail == null)
            {
                dataAndon.Antrian = "sukses";
                var dataAndonNew = _context.DataAndons.Where(a => a.Antrian == "prosess" && a.Id != dataAndon.Id).OrderBy(a => a.CreatedAt).FirstOrDefault();
                _context.DataAndons.Update(dataAndon);
                if (dataAndonNew != null) {
                    dataDetail = _context.DataAndonDetails.Where(a => a.IdAndon == dataAndonNew.Id && a.CountNumber == 0).FirstOrDefault();
                    if (dataAndon.StarProsess == null)
                    {
                        dataAndon.StarProsess = DateTime.Now;
                        _context.DataAndons.Update(dataAndon);

                    }
                    dataDetail.CountNumber = dataCount;
                    _context.DataAndonDetails.Update(dataDetail);
                }

            }
            else
            {

                dataDetail.CountNumber = dataCount;
                _context.DataAndonDetails.Update(dataDetail);
            }
           await _context.SaveChangesAsync(token);

        }

        //await _context.SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result> AddTimeStop(CancellationToken token  ,int dataTime)
    {
        try
        {
            
            DateTime limitTime = DateTime.Now.AddHours(-6);
            var dataList = await _context.DataAndons
                .Where(x => x.Status == 0 && x.CreatedAt >= limitTime)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            if (!dataList.Any())
                return Result.Failure("Tidak ada data yang memenuhi kriteria.");
            foreach (var item in dataList)
            {
                item.stopLine = item.stopLine+dataTime;
         
            }

            await _context.SaveChangesAsync(token);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Terjadi kesalahan: {ex.Message}");
        }
    }

    public async Task<Result> UpdateCounting( CancellationToken token ,bool status , int speed)
    {

        int max = _context.Settings.Max(a => a.EndRage).Value;

        var dataCounts = _context.DataCounts.FirstOrDefault();
        if (dataCounts != null)
        {
            if (!status)
            {
                dataCounts.CountNumber = dataCounts.CountNumber + 1;
                if (dataCounts.CountNumber + 1 > max + 1)
                    dataCounts.CountNumber = 1;
            }
            else
            {
                dataCounts.Speed = speed / 1000;
            }
     
            _context.DataCounts.Update(dataCounts);
            await _context.SaveChangesAsync(token);
        }
         
        return Result.Success();
    }

    
    public async Task<PaginatedList<DataAndonBriefDto>> GetDataAndonsAsync()
    {

        var data = await _context.DataAndons
           .Where(x => x.Status == 0)
           .OrderByDescending(x => x.CreatedAt)
           .ProjectTo<DataAndonBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1, 20);

        return data;
    }
    public async Task<List<DataAndonDetailBriefDto>> GetDataAndonAsync(CancellationToken token)
    {
        
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var dataCount = _context.DataCounts.FirstOrDefault()?.CountNumber ?? 0;

        var data = await _context.DataAndonDetails
            .Where(x => x.CreatedAt >= today && x.CreatedAt < tomorrow)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        var result = _context.Settings.AsQueryable();
        int max = result.Max(a => a.EndRage).Value;

        return data.Select(x => new DataAndonDetailBriefDto
        {
            Id = x.Id,
            PartName = x.PartName,
            PartNumber = x.PartNumber,
            Repair = x.Repair,
            LotMaterial = x.LotMaterial,
            Description = x.Description,
            CreatedAt = x.CreatedAt.ToString(),
            Coler = x.Coler,
            Qty = x.Qty,
            Locaton = DataLocation(DataLocationNo(dataCount, x.CountNumber ?? 0, x.Id, token, max), result).status,
            ColerLocation = DataLocation(DataLocationNo(dataCount, x.CountNumber ?? 0, x.Id, token, max), result).coler,

            LocatonNo = DataLocationNo(dataCount, x.CountNumber ?? 0, x.Id, token, max)

        }).OrderBy(a=> a.LocatonNo).ToList();
    }
    public async Task<PaginatedList<DataAndonDetailBriefDto>> GetDataAndonPafinationAsync(CancellationToken token)
    {

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var dataCount = _context.DataCounts.FirstOrDefault()?.CountNumber ?? 0;

        var settingsQuery = _context.Settings.AsQueryable();
        int max = settingsQuery.Max(a => a.EndRage).Value;

        var query = _context.DataAndonDetails
            .Where(x => x.CreatedAt >= today && x.CreatedAt < tomorrow)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DataAndonDetailBriefDto
            {
                Id  = x.Id,
                PartName = x.PartName,
                PartNumber = x.PartNumber,
                Repair = x.Repair,
                LotMaterial = x.LotMaterial,
                Description = x.Description,
                Coler = x.Coler,
                CreatedAt = x.CreatedAt.ToString(),
                LocatonNo = DataLocationNo(
                    dataCount,
                    x.CountNumber ?? 0,
                    x.Id,
                    token,
                    max
                ),
                Locaton = DataLocation(
                    DataLocationNo(
                        dataCount,
                        x.CountNumber ?? 0,
                        x.Id,
                        token,
                        max
                    ),
                    settingsQuery
                ).status,
                ColerLocation = DataLocation(
                    DataLocationNo(
                        dataCount,
                        x.CountNumber ?? 0,
                        x.Id,
                        token,
                        max
                    ),
                    settingsQuery
                ).coler
            })
            .OrderBy(x => x.LocatonNo);

        return await query.PaginatedListAsync(pageNumber: 1, pageSize: 20);

    }

    public int DataLocationNo(int countAct, int countDetail, Guid id, CancellationToken token, int max)
    {
        int nilai;
        if (countDetail == 0)
            return 0;
        if (countDetail >max)
            return countDetail;
        if (countDetail <= countAct)
            nilai = countAct - countDetail + 1;
        else
            nilai = countAct + max - countDetail + 1;
        if(nilai == max)
        {
           var data = _context.DataAndonDetails.Find(id);
            data.CountNumber = max + 1 ;
            _context.DataAndonDetails.Update(data);
            _context.SaveChangesAsync(token);

        }

        return nilai;
    }
    //public string DataLocation(int nilai )
    //{

    //    var data = _context.Settings.AsQueryable();
    //    return nilai switch
    //    {
    //        <= 0 => "loading",
    //        <= 4 => "treatment",
    //        <= 6 => "dry",
    //        <= 8 => "spray",
    //        <= 10 => "unloading",

    //        _ => "unloading"
    //    };
    //}

    public (string status , string coler) DataLocation(int nilai, IQueryable<Setting> data)
    {
        //if (nilai == 0)
        //    return ( "NEW", " #0000FF");
        var result = data
            .Where(x =>
                x.StartRage <= nilai &&
                x.EndRage >= nilai
            )
            //.Select(x => x.Name)
            .FirstOrDefault();

        return (result?.Name ?? "UNLOADING", result?.Coler?? "#ADB5BD");
    }
    public async Task<(byte[] fileBytes, string fileName, string contentType)> ExportDataAndonByDateAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            // **DEBUG: Log input parameters**
            Console.WriteLine($"=== DEBUG ExportDataAndonByDateAsync ===");
            Console.WriteLine($"Input StartDate: {startDate} (Kind: {startDate.Kind})");
            Console.WriteLine($"Input EndDate: {endDate} (Kind: {endDate.Kind})");

            // **Approach 1: Simple Date Range (Recommended)**
            var utcStartDate = startDate.Date; // 00:00:00
            var utcEndDate = endDate.Date.AddDays(1); // 00:00:00 next day (exclusive)

            Console.WriteLine($"UTC StartDate: {utcStartDate}");
            Console.WriteLine($"UTC EndDate: {utcEndDate}");
            Console.WriteLine($"Date Range: {utcStartDate:yyyy-MM-dd} TO {utcEndDate:yyyy-MM-dd}");

            // **DEBUG: Check total data in database**
            var totalDataInDb = await _context.DataAndons.CountAsync();
            var totalDataWithStatus1 = await _context.DataAndons.CountAsync(x => x.Status == 1);
            Console.WriteLine($"Total data in DB: {totalDataInDb}");
            Console.WriteLine($"Total data with Status=1: {totalDataWithStatus1}");

            // **Get the data**
            var data = await _context.DataAndons
                .Where(x => x.CreatedAt >= utcStartDate && x.CreatedAt < utcEndDate && x.Status == 1)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            Console.WriteLine($"Data found: {data.Count}");

            // **DEBUG: Show sample dates from result**
            if (data.Any())
            {
                Console.WriteLine("Sample dates from result:");
                foreach (var item in data.Take(5))
                {
                    Console.WriteLine($"- {item.CreatedAt:yyyy-MM-dd HH:mm:ss} (Status: {item.Status})");
                }
            }

            if (!data.Any())
            {
                // **DEBUG: Check if there's any data outside the date range**
                var minDateInDb = await _context.DataAndons.MinAsync(x => (DateTime?)x.CreatedAt);
                var maxDateInDb = await _context.DataAndons.MaxAsync(x => (DateTime?)x.CreatedAt);
                Console.WriteLine($"Date range in DB: {minDateInDb} to {maxDateInDb}");

                throw new Exception($"Tidak ada data pada rentang tanggal {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}.");
            }

            return GenerateProfessionalCsvFile(data, startDate, endDate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
            throw new Exception($"Error in ExportDataAndonByDateAsync: {ex.Message}");
        }
    }

    private (byte[] fileBytes, string fileName, string contentType) GenerateProfessionalCsvFile(List<DataAndon> data, DateTime startDate, DateTime endDate)
    {
        var sb = new StringBuilder();

        // **FIX: Tambah informasi header sebagai comments**
        sb.AppendLine("# ANDON PRODUCTION REPORT");
        sb.AppendLine("# Export Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        sb.AppendLine("# Period: " + startDate.ToString("dd/MM/yyyy") + " - " + endDate.ToString("dd/MM/yyyy"));
        sb.AppendLine("# Total Records: " + data.Count);
        sb.AppendLine(""); // Empty line

        // Header CSV
        sb.AppendLine("Loading Time,Part Name,Part Number,Description,Lot Material,Color,Qty Part,Qty Hanger,Repair");

        // Data
        foreach (var item in data)
        {
            var line = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",{6},{7},\"{8}\"",
                item.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                item.PartName ?? "-",
                item.PartNumber ?? "-",
                item.Description ?? "-",
                item.LotMaterial ?? "-",
                item.Coler ?? "-",  // **NOTE: Typo? Coler vs Color?**
                item.QtyPart,
                item.QtyHangar,     // **NOTE: Hangar vs Hanger?**
                item.Repair?.ToString() ?? "-");

            sb.AppendLine(line);
        }

        var fileName = string.Format("Andon_Production_Report_{0:yyyyMMdd}_{1:yyyyMMdd}.csv", startDate, endDate);
        var fileBytes = Encoding.UTF8.GetBytes(sb.ToString());

        Console.WriteLine($"File generated: {fileName}, Size: {fileBytes.Length} bytes");

        return (fileBytes, fileName, "text/csv; charset=utf-8");
    }

    
}
