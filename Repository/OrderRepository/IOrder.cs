using FinalProject.Models;

namespace FinalProject.Repository.OrderRepository
{
    public interface IOrder
    {

        string BuyNow(int userId);
        int GetTotalAmount(int userId);
        bool MatchProductId ( int userId );
        string BuyNowByOrderId(int userId, int productId);
        bool CheckQuantity(int userId, int productId);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetMyOrders(int userId);



    }
}
