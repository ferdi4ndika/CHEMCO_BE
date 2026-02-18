using MiniSkeletonAPI.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Images.Commands.DeleteImage;

public record DeleteImageCommand(Guid Id) : IRequest;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
    private readonly IIdentityImageService _context;

    public DeleteImageCommandHandler(IIdentityImageService context)
    {
        _context = context;
    }

    public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DeleteImageAsync(request.Id.ToString());
    }
}
