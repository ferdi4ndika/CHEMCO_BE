using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Commands.UpdateAllDataAndon;

public record UpdateAllDataAndonCommand : IRequest
{


}

public class UpdateAllDataAndonCommandHandler : IRequestHandler<UpdateAllDataAndonCommand>
{
    private readonly IIdentityDataAndonService _context;

    public UpdateAllDataAndonCommandHandler(
        IIdentityDataAndonService context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateAllDataAndonCommand request, CancellationToken cancellationToken)
    {
   
        var entity = await _context.UpdateAllDataAndonAsync();
    }
}