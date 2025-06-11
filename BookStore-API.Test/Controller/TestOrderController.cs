using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreAPI.Data.Entities;

namespace BookStore_API.Test.Controller
{
    [TestClass]
    public class TestOrderController
    {
        [TestMethod]
        public void GetAll_ShouldReturnAll()
        {
            var test = GetTestOrders();

        }
        private List<Order> GetTestOrders()
        {
            var testOrders = new List<Order>();

            testOrders.Add(new Order
            {
                StaffId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                PromotionId = Guid.NewGuid(),
                CreatedTime = DateTime.Now.AddDays(-2),
                TotalAmount = 100.123M,
                SubTotalAmount = 120.000M,
                PromotionAmount = 19.877M,
                Status = true,
                Note = "First test order"
            });

            testOrders.Add(new Order
            {
                StaffId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                PromotionId = null,
                CreatedTime = DateTime.Now.AddDays(-1),
                TotalAmount = 250.555M,
                SubTotalAmount = 270.000M,
                PromotionAmount = 19.445M,
                Status = false,
                Note = "Second test order with no promotion"
            });

            testOrders.Add(new Order
            {
                StaffId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                PromotionId = Guid.NewGuid(),
                CreatedTime = DateTime.Now,
                TotalAmount = 75.000M,
                SubTotalAmount = 80.000M,
                PromotionAmount = 5.000M,
                Status = true,
                Note = "Third test order"
            });

            return testOrders;
        }
    }
}
