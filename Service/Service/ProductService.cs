using FinalProject.Models;
using FinalProject.Repository.ProductRepository;
using FinalProject.Service.IService;

namespace FinalProject.Service.Service
{
    public class ProductService : IProductService
    {
        private readonly IProduct _productRepository;

        public ProductService(IProduct productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                return await _productRepository.GetAllProducts();   
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            try
            {
                return await _productRepository.GetProductsByCategoryId(categoryId);
            }
            catch(Exception ex)
            {
                throw ;
            }
        }

        public bool AddProduct(Product product)
        {
            // Check if the category exists
            var category = _productRepository.AddProduct(product);
            if (category == null)
            {
                throw new Exception($"Category with ID {product.CategoryId} not found.");
            }

            return true;
        }

        public async Task<Product> UpdateProduct(int id, Product product)
        {
            // Check if the product exists
            var existingProduct = await _productRepository.GetProductById(id);
            if (existingProduct == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            // Check if the category exists
            var category = await _productRepository.GetCategoriesByCategoryId(product.CategoryId);
            if (category == null)
            {
                throw new Exception($"Category with ID {product.CategoryId} not found.");
            }

            return await _productRepository.UpdateProduct(id, product);
        }

        public async Task<Product> DeleteProduct(int id)
        {
            // Check if the product exists
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            return await _productRepository.DeleteProduct(id);
        }

        public bool ProductExists(int id)
        {
            return _productRepository.ProductExists(id);
        }

        public async Task<IEnumerable<Product>> SearchProduct(string productString)
        {
            try
            {
                return await _productRepository.SearchProduct(productString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Category> GetCategoriesByCategoryId(int categoryId)
        {
            var category = await _productRepository.GetCategoriesByCategoryId(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with ID {categoryId} not found.");
            }
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                return await _productRepository.GetAllCategories();
            }catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Category> AddCategory(Category category)
        {
            try
            {
                return await _productRepository.AddCategory(category);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<Category> UpdateCategory(int id, Category category)
        {
            // Check if the category exists
            var existingCategory = await _productRepository.GetCategoriesByCategoryId(id);
            if (existingCategory == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }

            return await _productRepository.UpdateCategory(id, category);
        }

        public async Task<Category> DeleteCategory(int id)
        {
            // Check if the category exists
            var category = await _productRepository.GetCategoriesByCategoryId(id);
            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }

            return await _productRepository.DeleteCategory(id);
        }

        public bool CategoryExists(int id)
        {
            try
            {
                return _productRepository.CategoryExists(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

     
        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            // Check if the category exists
            var category = await _productRepository.GetCategoriesByCategoryId(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with ID {categoryId} not found.");
            }

            return await _productRepository.ProductByCategory(categoryId);
        }
    }
}
