﻿using FinalProject.Models;

namespace FinalProject.Service.IService
{
    public interface ICartService
    {

        Task<Cart> AddToCart(int productId, int userId);
        List<Cart> GetAllCartItems(int userId);
        int GetToTalPrice(int userId);
  
        bool RemoveItem(int productId, int userId);
        bool RemoveItem2(int productId);

        void ReduceItem(int userId, int productId);
        void IncreaseItem(int userId, int productId);
        string UpdateItem(int userId, int productId, int quantity);

        void EmptyCart(int userId);


        Task<int> GetCount(int userId);
    }
}
