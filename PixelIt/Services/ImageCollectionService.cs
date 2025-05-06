using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.DTOs.ImageCollection;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class ImageCollectionService
    {
        private readonly ApplicationDbContext _context;
        public ImageCollectionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> TryImageCollectionSaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
      
        public async Task<bool>? CreateImageCollection(ImageCollectionSimpleDto createImageCollection)
        {
            try
            {
                var fileName1 = createImageCollection.Image1.FileName;
                var fileName2 = createImageCollection.Image2.FileName;
                var fileName3 = createImageCollection.Image3.FileName;
                var fileName4 = createImageCollection.Image4.FileName;
                var fileName5 = createImageCollection.Image5.FileName;
                var uniqueFileName1 = Guid.NewGuid() + "_" + fileName1;
                var uniqueFileName2 = Guid.NewGuid() + "_" + fileName2;
                var uniqueFileName3 = Guid.NewGuid() + "_" + fileName3;
                var uniqueFileName4 = Guid.NewGuid() + "_" + fileName4;
                var uniqueFileName5 = Guid.NewGuid() + "_" + fileName5;
                var filePath1 = Path.Combine("wwwroot", "images collection", uniqueFileName1);
                var filePath2 = Path.Combine("wwwroot", "images collection", uniqueFileName2);
                var filePath3 = Path.Combine("wwwroot", "images collection", uniqueFileName3);
                var filePath4 = Path.Combine("wwwroot", "images collection", uniqueFileName4);
                var filePath5 = Path.Combine("wwwroot", "images collection", uniqueFileName5);
                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    await createImageCollection.Image1.CopyToAsync(stream);
                }
                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await createImageCollection.Image2.CopyToAsync(stream);
                }
                using (var stream = new FileStream(filePath3, FileMode.Create))
                {
                    await createImageCollection.Image3.CopyToAsync(stream);
                }
                using (var stream = new FileStream(filePath4, FileMode.Create))
                {
                    await createImageCollection.Image4.CopyToAsync(stream);
                }
                using (var stream = new FileStream(filePath5, FileMode.Create))
                {
                    await createImageCollection.Image5.CopyToAsync(stream);
                }

                var webRootPath1 = Path.Combine("wwwroot", "images collection", uniqueFileName1);
                var webRootPath2 = Path.Combine("wwwroot", "images collection", uniqueFileName2);
                var webRootPath3 = Path.Combine("wwwroot", "images collection", uniqueFileName3);
                var webRootPath4 = Path.Combine("wwwroot", "images collection", uniqueFileName4);
                var webRootPath5 = Path.Combine("wwwroot", "images collection", uniqueFileName5);


                var imageCollection = new ImageCollection
                {
                    IdImageCollection = Guid.NewGuid(),
                    Image1 = webRootPath1,
                    Image2 = webRootPath2,
                    Image3 = webRootPath3,
                    Image4 = webRootPath4,
                    Image5 = webRootPath5,
                };
                _context.ImageCollections.Add(imageCollection);
                return await TryImageCollectionSaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the image collection.", ex);
            }
            finally
            {
                await TryImageCollectionSaveAsync();
            }
        }
    }
}
