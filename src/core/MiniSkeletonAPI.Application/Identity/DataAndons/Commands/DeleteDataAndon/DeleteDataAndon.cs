using MiniSkeletonAPI.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Commands.DeleteDataAndon;

public record DeleteDataAndonCommand(Guid Id) : IRequest;

public class DeleteDataAndonCommandHandler : IRequestHandler<DeleteDataAndonCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDataAndonCommandHandler( IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDataAndonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DataAndons.FindAsync(request.Id);
        if (entity != null) {
             _context.DataAndons.Remove(entity);
            var dataDtail = _context.DataAndonDetails.Where(a => a.IdAndon == entity.Id).ToList();
            if(dataDtail.Count() > 0)
                _context.DataAndonDetails.RemoveRange(dataDtail);

            await  _context.SaveChangesAsync(cancellationToken);
        }
    }
}
