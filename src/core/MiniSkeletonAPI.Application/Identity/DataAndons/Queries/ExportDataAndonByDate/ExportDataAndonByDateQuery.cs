using MediatR;
using MiniSkeletonAPI.Application.Common.Models;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Queries.ExportDataAndonByDate
{
    public class ExportDataAndonByDateQuery : IRequest<(byte[] fileBytes, string fileName, string contentType)>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}