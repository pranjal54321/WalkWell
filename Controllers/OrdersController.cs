using FinalProject.Models;
using FinalProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
  
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService ordercontext;
        public OrdersController(IOrderService ordercontext)
        {
            this.ordercontext = ordercontext;

        }
        [HttpPost]
        [Route("AddOrder")]
        [Authorize(Roles = "Customer")]

        public IActionResult BuyNow()
        {

            var result =  ordercontext.BuyNow(GetIdFromToken());
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();

        }

        [HttpPost]
        [Route("AddOrderById")]
        [Authorize(Roles = "Customer")]
        public IActionResult BuyNowOrderById(int productId)
        {

            var result = ordercontext.BuyNowByOrderId( GetIdFromToken() ,productId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();

        }
      

        [HttpGet]
        [Route("GetTotalAmount")]

       
        private int GetIdFromToken()
        {
            var userIdClaim = HttpContext.User.FindFirst("userId");
            if (userIdClaim == null)
            {
                throw new Exception("User ID claim not found in token.");
            }
            return int.Parse(userIdClaim.Value);
        }



        [HttpGet("AllOrders")]
       
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {

            var orders = ordercontext.GetAllOrders();
            if (orders == null)
            {
                return NotFound("No orders found.");
            }
            return Ok(orders);
        }
        [HttpGet("MyOrders")]
       
        public ActionResult<IEnumerable<Order>> GetMyOrders()
        {
            var result = ordercontext.GetMyOrders(GetIdFromToken());
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        private int getid()
        {
            var id = HttpContext.User.FindFirst("UserId").Value;
            return int.Parse(id);
        }

    }


}