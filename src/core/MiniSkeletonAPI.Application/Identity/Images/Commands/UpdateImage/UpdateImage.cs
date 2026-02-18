using Microsoft.AspNetCore.Http;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Images.Commands.UpdateImage;

public record UpdateImageCommand : IRequest
{
    public required Guid Id { get; init; }
    public IFormFile? Name { get; init; }
    public string? Code { get; init; }
}

public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand>
{
    private readonly IIdentityImageService _context;
    private readonly string _imageDirectoryPath;

    public UpdateImageCommandHandler(
        IIdentityImageService context

        )
    {
        _context = context;
        _imageDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/gambar");

        if (!Directory.Exists(_imageDirectoryPath))
        {
            Directory.CreateDirectory(_imageDirectoryPath);
        }
    }

    public async Task Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
    
         var existingImage = await _context.GetImageByIdAsyncdata(request.Id.ToString());
        if (existingImage == null)
        {
            throw new ArgumentException("Gambar dengan ID tersebut tidak ditemukan");
        }
      //  if (existingImage.Name != request.Name.FileName)
            if (request.Name != null)
        {

            var oldFilePath = Path.Combine(_imageDirectoryPath, existingImage.Name);
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Name.FileName);
            var imagePath = Path.Combine(_imageDirectoryPath, imageFileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await request.Name.CopyToAsync(stream, cancellationToken);
            }

            var Image = new Image
            {
                Code = request.Code,
                Name = imageFileName,
            };

            var entity = await _context.UpdateImageAsync(Image, request.Id.ToString());
        }
        else {
            var Image = new Image
            {
                Code = request.Code,
                Name = "null",
            };

            var entity = await _context.UpdateImageAsync(Image, request.Id.ToString());

        }
        
      
    }
}