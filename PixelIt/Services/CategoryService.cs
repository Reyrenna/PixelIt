using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.DTOs.Category;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> TryCategorySaveAsync()
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

        public async Task<List<GetCategoriesDto>> GetCategories()
        {
            try
            {
                var categoryList = await _context
                .Categories.Include(c => c.PostCategories)
                .ToListAsync();

                var categoryDtoList = categoryList
                    .Select(c => new GetCategoriesDto()
                    {
                        CategoryName = c.CategoryName,
                    }).ToList();

                return categoryDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task<bool> CreateCategory(CreateCategoryDto createCategory)
        {
            try
            {
                var category = new Category
                {
                    IdCategory = Guid.NewGuid(),
                    CategoryName = createCategory.CategoryName,
                };
                await _context.Categories.AddAsync(category);
                return await TryCategorySaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the category.", ex);
            }
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return false;
                }
                _context.Categories.Remove(category);
                return await TryCategorySaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }
    }
}
