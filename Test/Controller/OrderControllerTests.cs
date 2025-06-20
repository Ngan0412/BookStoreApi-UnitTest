﻿using System.Security.Claims;
using BookStoreAPI.Controllers;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Services.OrderService.DTOs;
using BookStoreAPI.Services.OrderService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace Test.Controller
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task Add_ReturnsOkResult_WithSuccessMessage()
        {
            // Arrange
            var mockService = new Mock<IOrderService>();

            var orderCreateDto = new OrderCreateDTO
            {
                CustomerId = Guid.NewGuid(),
                PromotionId = null,
                Items = new List<OrderItemCreateDTO>
                {
                    new OrderItemCreateDTO { BookId = Guid.NewGuid(), Quantity = 2 },
                    new OrderItemCreateDTO { BookId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            // Giả lập service AddAsync
            mockService
                .Setup(s => s.AddAsync(orderCreateDto, It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(true);

            var controller = new OrderController(mockService.Object);

            // Giả lập người dùng
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await controller.Add(orderCreateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Ép kiểu rõ ràng từ anonymous object (dễ test hơn)
            var json = JsonConvert.SerializeObject(okResult.Value);
            var response = JsonConvert.DeserializeObject<MessageResponse>(json);

            Assert.Equal("Order created successfully.", response.Message);
        }
        [Fact]
        public async Task Add_ThrowsArgumentException_WhenItemsAreEmpty()
        {
            // Arrange
            var mockService = new Mock<IOrderService>();
            var controller = new OrderController(mockService.Object);

            var invalidDto = new OrderCreateDTO
            {
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemCreateDTO>() // rỗng
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "staff-id")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Giả lập lỗi trong service
            mockService
                .Setup(s => s.AddAsync(invalidDto, It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new ArgumentException("Order must have at least one item."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => controller.Add(invalidDto));
        }

        [Fact]
        public async Task Add_ThrowsArgumentException_WhenCustomerIsInvalid()
        {
            var mockService = new Mock<IOrderService>();
            var controller = new OrderController(mockService.Object);

            var dto = new OrderCreateDTO
            {
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemCreateDTO>
            {
                new OrderItemCreateDTO { BookId = Guid.NewGuid(), Quantity = 1 }
            }
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "staff-id")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Giả lập service ném exception khi Customer không hợp lệ
            mockService
                .Setup(s => s.AddAsync(dto, It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new ArgumentException("Customer is invalid or has been deleted."));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => controller.Add(dto));
        }
        [Fact]
        public async Task Add_ThrowsKeyNotFoundException_WhenBookNotFound()
        {
            var mockService = new Mock<IOrderService>();
            var controller = new OrderController(mockService.Object);

            var dto = new OrderCreateDTO
            {
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemCreateDTO>
            {
                new OrderItemCreateDTO { BookId = Guid.NewGuid(), Quantity = 1 }
            }
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "staff-id")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            mockService
                .Setup(s => s.AddAsync(dto, It.IsAny<ClaimsPrincipal>()))
                .ThrowsAsync(new KeyNotFoundException("Book not found."));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => controller.Add(dto));
        }
        public class MessageResponse
        {
            public string Message { get; set; }
        }
    }
}
