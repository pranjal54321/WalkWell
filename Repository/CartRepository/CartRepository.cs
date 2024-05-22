using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace FinalProject.Repository.CartRepository
{
    public class CartRepository:ICart
    {

        private readonly FinalDbContext _context;
       
        public CartRepository(FinalDbContext context)
        {
            this._context = context;
        }

        public List<Cart> GetAllCartItems(int userId)
        {
            return _context.Carts.Where(t => t.UserId == userId).ToList();
        }
        public async Task<Cart> AddToCart(int productId, int userId)
        {

            var productExist = await _context.Carts.FirstOrDefaultAsync(t => t.ProductId == productId && t.UserId == userId);
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return null;
            }

            var stock = _context.Products.FirstOrDefault(c => c.ProductId == productId).Stock;
            if (stock != 0)
            {
                if (productExist == null)
                {
                    productExist = new Cart
                    {
                        ProductId = productId,
                        UserId = userId,
                        Quantity = 1,
                        ProductName = _context.Products.SingleOrDefault(t => t.ProductId == productId).ProductName,
                        UnitPrice = _context.Products.SingleOrDefault(t => t.ProductId == productId).Price

                    };
                    await _context.Carts.AddAsync(productExist);

                }
                else if (stock <= productExist.Quantity)
                {
                    return productExist;
                }
                else
                {
                    productExist.Quantity++;

                }
                await _context.SaveChangesAsync();
                return productExist;
            }
            else
            {
                return null;
            }
        }

        public bool RemoveItem(int productId, int userId)
        {
            var result = _context.Carts.FirstOrDefault(t => t.ProductId == productId && t.UserId == userId);
            if (result != null)
            {
                _context.Carts.Remove(result);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveItem2(int productId)
        {
            var result = _context.Carts.FirstOrDefault(t => t.ProductId == productId );
            if (result != null)
            {
                _context.Carts.Remove(result);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        public int GetToTalPrice(int userId)
        {
            int sum = 0;
            var items = GetAllCartItems(userId);
            foreach (var item in items)
            {
                sum += item.UnitPrice * item.Quantity;
            }
            return sum;
        }

        public string UpdateItem(int userId, int productId, int quantity)
        {
            var stock = _context.Products.FirstOrDefault(p => p.ProductId == productId).Stock;
            if (stock >= (quantity + 1))
            {
                var myItem = _context.Carts.FirstOrDefault(t => t.UserId == userId && t.ProductId == productId);
                if (myItem != null)
                {
                    myItem.Quantity = quantity;
                    _context.SaveChanges();

                }
                return "Cart is updated.";
            }
            else
            {
                return ("product out of stock.");
            }
        }

        public void EmptyCart(int userId)
        {
            var cartItems = _context.Carts.Where(
          c => c.UserId == userId);
            foreach (var cartItem in cartItems)
            {
                _context.Carts.Remove(cartItem);
            }
                 
            _context.SaveChanges();
        }

        public int GetCount(int userId)
        {
            var items = _context.Carts.Where(c => c.UserId == userId);
            int count = 0;
            foreach (var item in items)
            {
                count = count + item.Quantity;
            }
            return count;
        }
        public void ReduceItem(int userId, int productId)
        {
            var qty = _context.Carts.FirstOrDefault(c => c.ProductId == productId).Quantity;
            if (qty > 1)
            {
                _context.Carts.FirstOrDefault(c => c.ProductId == productId).Quantity = qty - 1;
                _context.SaveChanges();
            }
        }

        public void IncreaseItem(int userId, int productId)

        {

            var qty = _context.Carts.FirstOrDefault(c => c.ProductId == productId).Quantity;

            var stock = _context.Products.FirstOrDefault(p => p.ProductId == productId).Stock;

            if (qty <= stock)
            {
                _context.Carts.FirstOrDefault(c => c.ProductId == productId).Quantity = qty + 1;
                _context.SaveChanges();
            }

        }

    }
   




}

