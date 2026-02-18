using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Text.Json.Serialization;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Commands.CreateDataAndon
{
    public record CreateDataAndonCommand : IRequest<Guid>
    {
        [JsonPropertyName("coler")]
        public required string? Coler { get; init; }
        [JsonPropertyName("id_part")]
        public required Guid? IdType { get; init; }
        [JsonPropertyName("description")]
        public required string? Description { get; init; }
        [JsonPropertyName("lot_material")]
        public required string? LotMaterial { get; init; }

        [JsonPropertyName("repair")]
        public required string? Repair { get; init; }
        [JsonPropertyName("qty_part")]
        public required int? QtyPart { get; init; }
       
    }

    public class CreateDataAndonCommandHandler : IRequestHandler<CreateDataAndonCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateDataAndonCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDataAndonCommand request, CancellationToken cancellationToken)
        {

            //var dataPart = _context.Parts.Find(request.IdType);
            var dataPart = await _context.Parts.FindAsync(request.IdType!.Value,cancellationToken);
            //var makan = 1;
            //int qtyHangar = request.QtyPart / dataPart.Qty;
            float dataSpeed = (await _context.DataCounts
               .Select(x => x.Speed)
               .FirstOrDefaultAsync()) ?? 0f;


            var dataAndon = new DataAndon
            {
                Id = Guid.NewGuid(),
                Coler = request.Coler,
                PartName= dataPart?.PartName,
                PartNumber = dataPart?.PartNumber,
                Antrian = "prosess",
                IdType = request.IdType,
                Description = request.Description,
                Status = 0,
                LotMaterial = request.LotMaterial,
                HangerSpeed = dataSpeed,
                Repair = request.Repair,
                QtyPart = request.QtyPart,
                QtyHangar = (request.QtyPart +dataPart?.Qty -1)/dataPart?.Qty


            };
            List<DataAndonDetail> dataAndonDetails = new List<DataAndonDetail>();
                  int qtyPerHangar = dataPart?.Qty ?? 0;
                    int sisaQty = request.QtyPart ?? 0;

            for (int i = 0; i < dataAndon.QtyHangar; i++)
            {
                bool isLast = (i == dataAndon.QtyHangar - 1);

                int qtyCurrent;

                if (isLast)
                {
                    // hangar terakhir ambil sisa
                    qtyCurrent = sisaQty;
                }
                else
                {
                    // hangar normal
                    qtyCurrent = qtyPerHangar;
                    sisaQty -= qtyPerHangar;
                }

                var detailAndon = new DataAndonDetail
                {
                    Coler = dataAndon.Coler,
                    IdType = dataAndon.IdType,
                    PartName = dataAndon.PartName,
                    PartNumber = dataAndon.PartNumber,
                    Description = request.Description,
                    LotMaterial = request.LotMaterial,
                    Qty = qtyCurrent,
                    CountNumber = 0,
                    Repair = request.Repair,
                    IdAndon = dataAndon.Id 
                };

                 dataAndonDetails.Add(detailAndon);
}

              
         
            _context.DataAndons.Add(dataAndon);

            await  _context.DataAndonDetails.AddRangeAsync(dataAndonDetails);
   
            await _context.SaveChangesAsync(cancellationToken);

            return dataAndon.Id;
        }
    }
}
