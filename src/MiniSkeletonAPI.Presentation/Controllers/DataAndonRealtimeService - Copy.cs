using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.SignalR;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Presentation.Hubs;
using System.Formats.Asn1;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;


namespace MiniSkeletonAPI.Presentation.Controllers
{
    public class RealTimeHubExcelService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<RealTimeHubExcel> _hubContext;

        public RealTimeHubExcelService(IServiceScopeFactory scopeFactory, IHubContext<RealTimeHubExcel> hubContext)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dataAndonService = scope.ServiceProvider.GetRequiredService<IIdentityDataAndonService>();
                        try
                        {
                            //string folderPath = @"C:\Users\Ferdi\Downloads\dastaEx";
                            string folderPath = @"C:\Users\mindr\Downloads";
                            //string folderPath = @"\\192.168.2.189\data_painting\99. PAINT PERFORMA\# APRIL\PLAN APRIL";
                            var latestFile = Directory.GetFiles(folderPath, "*.xlsx")
                             .Select(f => new FileInfo(f))
                             .OrderByDescending(f => f.LastWriteTime)
                             .FirstOrDefault();

                            if (latestFile == null)
                            {
                                Console.WriteLine("Tidak ada file Excel ditemukan.");
                                return;
                            }

                            string filePath = latestFile.FullName;

                            using (var stream = new FileStream(
                                filePath,
                                FileMode.Open,
                                FileAccess.Read,
                                FileShare.ReadWrite))
                            {
                                using var workbook = new XLWorkbook(stream);
                                var worksheet = workbook.Worksheet("ACTUAL WIP PRINT ");
                                var records = new List<Dictionary<string, string>>();

                         
                                for (int row = 7; row <= worksheet.LastRowUsed().RowNumber(); row++)
                                {
                                    var rowDict = new Dictionary<string, string>
                                    {
                                        ["part_number"] = worksheet.Cell(row, 1).GetValue<string>(),
                                        ["no"] = worksheet.Cell(row, 2).GetValue<string>(),
                                        ["part_name"] = worksheet.Cell(row, 3).GetValue<string>(),
                                        ["lokasi"] = worksheet.Cell(row, 4).GetValue<string>(),
                                        ["warna"] = worksheet.Cell(row, 7).GetValue<string>(),
                                        ["ews_i"] = worksheet.Cell(row, 9).GetValue<string>(),
                                        ["ews_j"] = worksheet.Cell(row, 10).GetValue<string>(),
                                        ["status"] = worksheet.Cell(row, 13).GetValue<string>(),
                                        ["urutan"] = GetNilai(worksheet.Cell(row, 13).GetValue<string>()).ToString(),
                                        //["N"] = worksheet.Cell(row, 14).GetValue<string>(),
                                        ["wip"] = worksheet.Cell(row, 16).GetValue<string>(),
                                        ["material"] = worksheet.Cell(row, 33).GetValue<string>(),
                                        ["Update_bc"] = worksheet.Cell(row, 39).GetValue<string>(),
                                        ["update_fg"] = worksheet.Cell(row, 45).GetValue<string>(),
                                        ["shift"] = worksheet.Cell(row, 67).GetValue<string>(),
                                        ["hangar"] = worksheet.Cell(row, 69).GetValue<string>()
                                    };
                                    records.Add(rowDict);
                                }

                                Console.WriteLine(records);
                                if (records != null)
                                {
                                    await _hubContext.Clients.All.SendAsync("ReceiveDataAndon", records.OrderBy(a=> a["urutan"]), cancellationToken: stoppingToken);
                                    Console.WriteLine("Data berhasil dikirim ke klien.");
                                }
                                else
                                {
                                    Console.WriteLine("Data kosong atau tidak tersedia.");
                                }

                            }
                       

                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR:");
                            Console.WriteLine(ex.Message);
                        }
                       


                       
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ [SignalR Error] {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

           
        }
        public static int GetNilai(string status)
        {
            switch (status)
            {
                case "Darkness":
                    return (int)Status.Darkness;

                case "X (Top Urgent)":
                    return (int)Status.X1;

                case "∆ (Urgent)":
                    return (int)Status.X2;

                case "Potensi Masalah":
                    return (int)Status.PotensiMasalah;

                case "OK":
                    return (int)Status.OK;

                default:
                    return 5;
            }
        }
    }
    public enum Status
    {
        Darkness = 0,
        X1 = 1,
        X2 = 2,
        PotensiMasalah = 3,
        OK = 4
    }
}
