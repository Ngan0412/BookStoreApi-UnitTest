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
    }
}
