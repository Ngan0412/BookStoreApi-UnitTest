using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.OrderService;
using BookStoreAPI.Services.OrderService.DTOs;
using BookStoreAPI.Services.OrderService.Repositories;
using Moq;

namespace Test.Service
{
    public class OrderServiceTest
    {
        [Fact]
        public async Task AddAsync_WithValidOrderCreateDTO_ReturnsTrue()
        {
            OrderCreateDTO orderCreateDTO = new OrderCreateDTO()
            {
                PromotionId = null,
                CustomerId = new Guid(),
                Items = new List<OrderItemCreateDTO>()
                {
                    new OrderItemCreateDTO()
                    {
                        BookId = new Guid(),
                        Quantity = 1
                    },
                    new OrderItemCreateDTO()
                    {
                        BookId = new Guid(),
                        Quantity = 2
                    }
                }
            };

            var mockRepository = new Mock<IOrderRepository>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                 new Claim("StaffId", Guid.NewGuid().ToString()),
            }, "mock"));
            
            Customer customer = new Customer()
            {
                Id = orderCreateDTO.CustomerId,
                FamilyName = "Thuy",
                GivenName = "Ngan",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                Address = "97 man thien",
                Phone = "0982286126",
                Gender = false,
            };

            Book book = new Book
            {
                Id = orderCreateDTO.Items[0].BookId,
                Isbn = "9781234567890",
                Title = "Test Book Title",
                CategoryId = Guid.NewGuid(),
                AuthorId = Guid.NewGuid(),
                PublisherId = Guid.NewGuid(),
                YearOfPublication = 2023,
                Price = 150000,
                Image = "book-image.jpg",
                Quantity = 10
            };
            Book book2 = new Book
            {
                Id = orderCreateDTO.Items[1].BookId,
                Isbn = "9781234567890",
                Title = "Test Book Title",
                CategoryId = Guid.NewGuid(),
                AuthorId = Guid.NewGuid(),
                PublisherId = Guid.NewGuid(),
                YearOfPublication = 2023,
                Price = 150000,
                Image = "book-image.jpg",
                Quantity = 10
            };
            mockRepository.Setup(x => x.GetCustomerByIdAsync(customer.Id)).ReturnsAsync(customer);
            mockRepository.Setup(x => x.GetBookByIdAsync(book.Id)).ReturnsAsync(book);
            mockRepository.Setup(x => x.GetBookByIdAsync(book2.Id)).ReturnsAsync(book2);
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var orderService = new OrderService(mockRepository.Object);
            
            var result = await orderService.AddAsync(orderCreateDTO, user);

            Assert.True(result);
            mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task AddAsync_WithNullItems_ThrowsArgumentException()
        {
            var orderService = new OrderService(Mock.Of<IOrderRepository>());

            var dto = new OrderCreateDTO
            {
                Items = null,
                CustomerId = Guid.NewGuid()
            };

            var user = new ClaimsPrincipal();

            await Assert.ThrowsAsync<ArgumentException>(() => orderService.AddAsync(dto, user));
        }
        [Fact]
        public async Task AddAsync_WithEmptyItems_ThrowsArgumentException()
        {
            var orderService = new OrderService(Mock.Of<IOrderRepository>());

            var dto = new OrderCreateDTO
            {
                Items = new List<OrderItemCreateDTO>(),
                CustomerId = Guid.NewGuid()
            };

            var user = new ClaimsPrincipal();

            await Assert.ThrowsAsync<ArgumentException>(() => orderService.AddAsync(dto, user));
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task AddAsync_InvalidCustomer_ThrowsArgumentException(bool isDeleted)
        {
            var mockRepo = new Mock<IOrderRepository>();
            var customerId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetCustomerByIdAsync(customerId))
                    .ReturnsAsync(isDeleted ? new Customer { Id = customerId, IsDeleted = true } : null);

            var dto = new OrderCreateDTO
            {
                CustomerId = customerId,
                Items = new List<OrderItemCreateDTO> { new() { BookId = Guid.NewGuid(), Quantity = 1 } }
            };

            var service = new OrderService(mockRepo.Object);
            var user = new ClaimsPrincipal();

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(dto, user));
        }
        [Fact]
        public async Task AddAsync_BookNotFound_ThrowsKeyNotFoundException()
        {
            var mockRepo = new Mock<IOrderRepository>();
            var customerId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetCustomerByIdAsync(customerId))
                    .ReturnsAsync(new Customer { Id = customerId });

            mockRepo.Setup(r => r.GetBookByIdAsync(bookId))
                    .ReturnsAsync((Book)null);

            var dto = new OrderCreateDTO
            {
                CustomerId = customerId,
                Items = new List<OrderItemCreateDTO> { new() { BookId = bookId, Quantity = 1 } }
            };

            var service = new OrderService(mockRepo.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("StaffId", Guid.NewGuid().ToString()) }));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddAsync(dto, user));
        }
        [Fact]
        public async Task AddAsync_BookQuantityNotEnough_ThrowsInvalidOperationException()
        {
            var mockRepo = new Mock<IOrderRepository>();
            var customerId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetCustomerByIdAsync(customerId))
                    .ReturnsAsync(new Customer { Id = customerId });

            mockRepo.Setup(r => r.GetBookByIdAsync(bookId))
                    .ReturnsAsync(new Book { Id = bookId, Quantity = 0, Price = 100 });

            var dto = new OrderCreateDTO
            {
                CustomerId = customerId,
                Items = new List<OrderItemCreateDTO> { new() { BookId = bookId, Quantity = 1 } }
            };

            var service = new OrderService(mockRepo.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("StaffId", Guid.NewGuid().ToString()) }));

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(dto, user));
        }
        [Fact]
        public async Task AddAsync_WithValidPromotion_AppliesDiscount()
        {
            var customerId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var promotionId = Guid.NewGuid();

            var customer = new Customer { Id = customerId };
            var book = new Book { Id = bookId, Price = 100, Quantity = 10 };

            var promotion = new Promotion
            {
                Id = promotionId,
                Quantity = 10,
                DiscountPercent = 0.1m,
                Condition = 50,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1)
            };

            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(r => r.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);
            mockRepo.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            mockRepo.Setup(r => r.GetPromotionByIdAsync(promotionId)).ReturnsAsync(promotion);
            mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var dto = new OrderCreateDTO
            {
                CustomerId = customerId,
                PromotionId = promotionId,
                Items = new List<OrderItemCreateDTO> { new() { BookId = bookId, Quantity = 1 } }
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim("StaffId", Guid.NewGuid().ToString())
            }));

            var service = new OrderService(mockRepo.Object);
            var result = await service.AddAsync(dto, user);

            Assert.True(result);
            Assert.Equal(9, promotion.Quantity); // Kiểm tra giảm số lượng
        }
    }
}
