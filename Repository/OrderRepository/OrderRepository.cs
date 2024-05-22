using FinalProject.Models;
using FinalProject.Repository.CartRepository;
using FinalProject.Repository.ProductRepository;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository.OrderRepository
{
    public class OrderRepository : IOrder
    {

        private readonly FinalDbContext _context;
        private readonly ICart _cart;
        private readonly IProduct _product;

        public OrderRepository(FinalDbContext context, ICart _cart, IProduct product)
        {
            this._context = context;
            this._cart = _cart;
            this._product = product;
        }
        public int GetTotalAmount(int userId)
        {
            return _cart.GetToTalPrice(userId);
        }
      
        public bool MatchProductId(int userId)
        {
            int cartProductId = _context.Carts.FirstOrDefault(c => c.UserId == userId).ProductId;
            var productProductId = _context.Products.FirstOrDefault(c => c.ProductId == cartProductId).ProductId;

            if (productProductId != null)
            {
                return true;
            }
            return false;
        }
        public bool Minus(int productId, int userId)
        {
            var result = _context.Products.FirstOrDefault(c => c.ProductId == productId);
            var qty = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId).Quantity;
            result.Stock = result.Stock - Convert.ToInt32(qty);

            _context.SaveChanges();

            return true;
        }
        public int GetAmountById(int userId, int productId)
        {
            int sum = 0;
            var result = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId).UnitPrice;
            var result1 = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId).Quantity;
            return result * result1;
        }

        public bool CheckQuantity(int userId, int productId)
        {
            var result = _context.Products.FirstOrDefault(c => c.ProductId == productId).Stock;
            var qty = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId).Quantity;
            int remain = result - Convert.ToInt32(qty);
            if (remain > 0)
            {
                return true;
            }
            return false;
        }
        public string BuyNowByOrderId(int userId, int productId)
        {

            bool check = CheckQuantity(userId, productId);

            if (check == false)
            {
                return "Out of stock";
            }

            else
            {
                var Id = _context.Carts.FirstOrDefault(c => c.ProductId == productId).ProductId;
                bool m = MatchProductId(userId);

                if (m == true && Id == productId)
                {

                    var order = new Order
                    {
                        UserId = userId,

                        TotalAmount = GetAmountById(userId, productId)

                       
                    };

                    _context.Orders.Add(order);

                    _context.SaveChanges();

                    Minus(productId, userId);

                    _cart.EmptyCart(userId);

                    return "your order is done.\nYour Order id is : " + _context.Orders.FirstOrDefault(c => c.UserId == userId).OrderId.ToString();
                }
                else
                {
                    return "Product Id not found";
                }

            }
        }
        public string BuyNow(int userId)
        {
            var carts = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (carts == null)
            {
                return "Cart is empty.";
            }
            else
            {
                var Id = _context.Carts.Where(p => p.UserId == userId).Select(p => p.ProductId).ToList();
                bool[] arr = new bool[Id.Count];
                int count = 0;
                foreach (var id in Id)
                {
                    if (_context.Carts.FirstOrDefault(p => p.ProductId == id && p.UserId == userId).Quantity <= _context.Products.FirstOrDefault(p => p.ProductId == id).Stock)
                    {
                        arr[count] = true;
                        count++;
                    }
                    else
                    {
                        arr[count] = false;
                    }
                }
                if (arr.Contains(false))
                {
                    return "out of stock.";
                }
                else
                {
                    int total = 0;
                    int p = 0;
                    foreach (var id in Id)
                    {

                        int amount = _context.Products.FirstOrDefault(p => p.ProductId == id).Price;
                        int quantity = _context.Carts.FirstOrDefault(u => u.ProductId == id).Quantity;
                        p = id;
                        int price = amount * quantity;
                        total += price;
                        Minus(p, userId);
                    }

                    var order = new Order
                    {
                        UserId = userId,
                        ProductId = carts.ProductId,

                        ProductName = carts.ProductName,
                        Quantity = carts.Quantity,

                        TotalAmount = total

                     
                    };

                    _context.Orders.Add(order);

                    _context.SaveChanges();

                    _cart.EmptyCart(userId);

                    return _context.Orders.FirstOrDefault(c => c.UserId == userId).OrderId.ToString();
                }

            }
        }


        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public IEnumerable<Order> GetMyOrders(int userId)
        {
            return _context.Orders.Where(t => t.UserId == userId).ToList();
        }

    }
}