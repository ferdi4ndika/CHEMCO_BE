using MiniSkeletonAPI.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataParts.Commands.DeletePart;

public record DeletePartCommand(Guid Id) : IRequest;

public class DeletePartCommandHandler : IRequestHandler<DeletePartCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePartCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePartCommand request, CancellationToken cancellationToken)
    {

        var entity = _context.Parts.Find(request.Id);
        if (entity != null)
        {
            _context.Parts.Remove(entity);
           await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
