using MiniSkeletonAPI.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMRepairs.Commands.DeleteMRepair;

public record DeleteMRepairCommand(Guid Id) : IRequest;

public class DeleteMRepairCommandHandler : IRequestHandler<DeleteMRepairCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMRepairCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMRepairCommand request, CancellationToken cancellationToken)
    {

        var entity = _context.MRepairs.Find(request.Id);
        if (entity != null) {
             _context.MRepairs.Remove(entity);
           await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
