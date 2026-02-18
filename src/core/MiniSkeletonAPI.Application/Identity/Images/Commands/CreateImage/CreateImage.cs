using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MiniSkeletonAPI.Application.Identity.Images.Commands.CreateImage
{
    public record CreateImageCommand : IRequest<Guid>
    {
        public IFormFile? Name { get; init; }
    }

    public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, Guid>
    {
        private readonly IIdentityImageService _context;
        private readonly string _imageDirectoryPath;

   
        public CreateImageCommandHandler(
            IIdentityImageService context)
        {
            _context = context;
            _imageDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/gambar");

            if (!Directory.Exists(_imageDirectoryPath))
            {
                Directory.CreateDirectory(_imageDirectoryPath);
            }
        }

        public async Task<Guid> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
           
            var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Name.FileName); 
            var imagePath = Path.Combine(_imageDirectoryPath, imageFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await request.Name.CopyToAsync(stream, cancellationToken);
            }

    
            var imageEntity = new Image
            {
   
                Name = imageFileName, 
            };

           
            var entity = await _context.CreateImageAsync(imageEntity);

            return entity.ImageId;
        }
    }
}
