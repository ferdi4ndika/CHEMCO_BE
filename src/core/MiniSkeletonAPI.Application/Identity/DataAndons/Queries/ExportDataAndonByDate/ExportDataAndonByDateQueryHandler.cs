using MediatR;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.DataAndons.Queries.ExportDataAndonByDate;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Queries.ExportDataAndonByDate
{
    public class ExportDataAndonByDateQueryHandler : IRequestHandler<ExportDataAndonByDateQuery, (byte[] fileBytes, string fileName, string contentType)>
    {
        private readonly IIdentityDataAndonService _dataAndonService;

        public ExportDataAndonByDateQueryHandler(IIdentityDataAndonService dataAndonService)
        {
            _dataAndonService = dataAndonService;
        }

        public async Task<(byte[] fileBytes, string fileName, string contentType)> Handle(ExportDataAndonByDateQuery request, CancellationToken cancellationToken)
        {
            // Pastikan datetime dalam UTC
            var utcStartDate = request.StartDate.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc)
                : request.StartDate.ToUniversalTime();

            var utcEndDate = request.EndDate.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc)
                : request.EndDate.ToUniversalTime();

            return await _dataAndonService.ExportDataAndonByDateAsync(utcStartDate, utcEndDate);
        }
    }
}