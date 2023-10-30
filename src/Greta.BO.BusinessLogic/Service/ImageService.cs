using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IImageService : IGenericBaseService<Image>
    {
        Task<Image> GetPrimaryImage(ImageType type, long ownerId);

        Task<List<Image>> Get(long ownerId, ImageType type);

        Task<Image> GetImage(long imageId);

        Task<Image> AddImage(Image image);

        Task<bool> DeleteImage(long image);
    }

    public class ImageService : BaseService<IImageRepository, Image>, IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(
            ILogger<ImageService> logger,
            IImageRepository imageRepository) : base(imageRepository, logger)
        {
            _imageRepository = imageRepository;
        }

        public async Task<Image> AddImage(Image image)
        {
            var id = await _imageRepository.CreateAsync(image);
            image.Id = id;
            return image;
        }

        public async Task<bool> DeleteImage(long image)
        {
            var id = await _imageRepository.DeleteAsync(image);
            return id;
        }

        public async Task<List<Image>> Get(long ownerId, ImageType type)
        {
            return await _imageRepository.GetEntity<Image>()
                .Where(x => x.Type == type && x.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<Image> GetImage(long imageId)
        {
            return await _imageRepository.GetEntity<Image>()
                .Where(x => x.Id == imageId)
                .FirstOrDefaultAsync();
        }

        public async Task<Image> GetPrimaryImage(ImageType type, long ownerId)
        {
            return await _imageRepository.GetEntity<Image>()
                .Where(
                    x => x.OwnerId == ownerId
                         && x.Type == type
                         && x.IsPrimary)
                .FirstOrDefaultAsync();
        }
    }
}