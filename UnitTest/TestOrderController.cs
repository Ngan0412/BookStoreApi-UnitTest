namespace UnitTest
{
    [TestClass]
    public class TestOrderController
    {
        [TestMethod]
        public void GetAll_ShouldReturnAll()
        {
            var testOrders = GetTestOrders();
            var controller = new OrderController();

            var result = controller.GetAll() as List<Product>;
            Assert.AreEqual(testProducts.Count, result.Count);
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
