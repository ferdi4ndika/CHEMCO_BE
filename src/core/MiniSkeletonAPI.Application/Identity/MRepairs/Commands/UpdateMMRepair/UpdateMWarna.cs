using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMRepairs.Commands.UpdateMRepair;

public record UpdateMRepairCommand : IRequest
{
    public required Guid Id { get; init; }
    [JsonPropertyName("repair")]
    public string? Repair { get; init; }


}

public class UpdateMRepairCommandHandler : IRequestHandler<UpdateMRepairCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMRepairCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateMRepairCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.MRepairs.Find(request.Id);
        if (entity != null) { 
           entity.Repair =request.Repair;
            _context.MRepairs.Update(entity);
            _context.SaveChangesAsync(cancellationToken);
       }
           
    }
}