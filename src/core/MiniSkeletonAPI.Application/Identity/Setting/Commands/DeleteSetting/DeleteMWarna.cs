using MiniSkeletonAPI.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MasterdataMasterdataSettings.Commands.DeleteSetting;

public record DeleteSettingCommand(Guid Id) : IRequest;

public class DeleteSettingCommandHandler : IRequestHandler<DeleteSettingCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSettingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
    {

        var entity = _context.Settings.Find(request.Id);
        if (entity != null) {
             _context.Settings.Remove(entity);
           await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
