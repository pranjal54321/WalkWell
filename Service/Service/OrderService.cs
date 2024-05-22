using FinalProject.Models;
using FinalProject.Repository.OrderRepository;
using FinalProject.Service.IService;

namespace FinalProject.Service.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrder _orderRepository;

        public OrderService(IOrder ordercontext)
        {
            this._orderRepository = ordercontext;

        }

        public string BuyNow(int userId)
        {
           
            try
            {
               return _orderRepository.BuyNow(userId); 
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BuyNow(): {ex.Message}");
                throw;
            }

        }


        public string BuyNowByOrderId(int userId, int productId)
        {

            try
            {
                return _orderRepository.BuyNowByOrderId(userId ,productId);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BuyNow(): {ex.Message}");
                throw;
            }
           
            

        }
       
        public IEnumerable<Order> GetAllOrders()
        {
          
            var orders = _orderRepository.GetAllOrders();
            if (orders == null)
            {
                throw new Exception("No order in the Database");
            }
            return  _orderRepository.GetAllOrders();
        }
   
        public IEnumerable<Order> GetMyOrders(int userId)
        {
            
            var orders = _orderRepository.GetMyOrders(userId);
            if (orders == null)
            {
                throw new Exception("No order in the  database");
            }
            return  orders;
        }


    }
}
