using MiniSkeletonAPI.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMWarnas.Commands.DeleteMWarna;

public record DeleteMWarnaCommand(Guid Id) : IRequest;

public class DeleteMWarnaCommandHandler : IRequestHandler<DeleteMWarnaCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMWarnaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMWarnaCommand request, CancellationToken cancellationToken)
    {

        var entity = _context.MWarnas.Find(request.Id);
        if (entity != null) {

            _context.MWarnas.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
 
    }
}
