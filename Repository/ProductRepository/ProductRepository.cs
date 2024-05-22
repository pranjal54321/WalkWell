using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository.ProductRepository
{
    public class ProductRepository:IProduct
    {
        private readonly FinalDbContext _context;

        public ProductRepository(FinalDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(t => t.ProductId == id);
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _context.Products.Where(t => t.CategoryId == categoryId).ToListAsync();
        }
      

        public bool AddProduct(Product product)
        {
            var res = _context.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            if (product.CategoryId == 0 || res == null)
            {
                return false;
            }
            var result = _context.Products.Add(product);
            _context.SaveChanges();
            return true;
        }
        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var result = await _context.Products.FirstOrDefaultAsync(t => t.ProductId == id);
            if (result != null)
            {
                result.ProductName = product.ProductName;
                result.ProductDescription = product.ProductDescription;
                result.Price = product.Price;
                result.Stock = product.Stock;
                result.ImageURL = product.ImageURL;
                result.CategoryId = product.CategoryId;

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(t => t.ProductId == id);
        }

        public async Task<IEnumerable<Product>> SearchProduct(string productString)
        {
            var result = _context.Products.Where(p => p.ProductName.Contains(productString) || p.ProductDescription.Contains(productString));
            if (result != null)
            {
                return result;
            }
            return null;
        }

      

        public async Task<Category> GetCategoriesByCategoryId(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
        public async Task<List<Product>> ProductByCategory(int CategoryId)
        {
            string FilterByCategoryId = "exec ProductByCategory @Categoryid =" + CategoryId;
            return _context.Products.FromSqlRaw(FilterByCategoryId).ToList();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> AddCategory(Category category)
        {
            var result = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var res=await _context.Products.FirstOrDefaultAsync(t=>t.ProductId== id);
            if(res != null)
            {
                _context.Products.Remove(res);
                await _context.SaveChangesAsync();
                return res;
            }
            return null;
        }
     
        public async Task<Category> UpdateCategory(int id, Category category)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(t => t.CategoryId == id);
            if (result != null)
            {
                result.CategoryName = category.CategoryName;

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(t => t.CategoryId == id);
            if (result != null)
            {
                _context.Categories.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(t => t.CategoryId == id);
        }
    }
}
