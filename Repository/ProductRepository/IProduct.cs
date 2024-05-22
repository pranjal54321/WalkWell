using FinalProject.Models;

namespace FinalProject.Repository.ProductRepository
{
    public interface IProduct
    {

        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);
        bool AddProduct(Product product);
        Task<Product> UpdateProduct(int id, Product product);
        Task<Product> DeleteProduct(int id);


        bool ProductExists(int id);
        Task<IEnumerable<Product>> SearchProduct(string productString);
        Task<Category> GetCategoriesByCategoryId(int categoryId);



        Task<IEnumerable<Category>> GetAllCategories();

        Task<Category> AddCategory(Category category);
        Task<List<Product>> ProductByCategory(int CategoryId);

        Task<Category> UpdateCategory(int id, Category category);

        Task<Category> DeleteCategory(int id);

        bool CategoryExists(int id);
    }
}
