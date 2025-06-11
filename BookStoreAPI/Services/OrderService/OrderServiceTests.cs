namespace BookStoreAPI.Services.OrderService;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStoreAPI.Services.OrderService.Repositories;
using BookStoreAPI.Services.OrderService.DTOs;
using BookStoreAPI.Data.Entities;

[TestFixture]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _repo;
    private OrderService _service;
    private ClaimsPrincipal _user;

    [SetUp]
    public void Setup()
    {
        _repo = new Mock<IOrderRepository>();
        _service = new OrderService(_repo.Object);
        _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("StaffId", "staff-id")
        }));
    }

    [Test]
    public void AddAsync_ShouldThrow_WhenNoItems()
    {
        var dto = new OrderCreateDTO { Items = null };

        var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto, _user));
        Assert.That(ex.Message, Is.EqualTo("Order must have at least one item."));
    }

    [Test]
    public void AddAsync_ShouldThrow_WhenCustomerInvalid()
    {
        var dto = new OrderCreateDTO
        {
            Items = new List<OrderItemCreateDTO> { new OrderItemCreateDTO { BookId = Guid.NewGuid(), Quantity = 1 } },
            CustomerId = Guid.NewGuid()
        };

        _repo.Setup(r => r.GetCustomerByIdAsync(dto.CustomerId)).ReturnsAsync((Customer)null);

        var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto, _user));
        Assert.That(ex.Message, Is.EqualTo("Customer is invalid or has been deleted."));
    }

    [Test]
    public void AddAsync_ShouldThrow_WhenBookNotFound()
    {
        var bookId = Guid.NewGuid();
        var dto = new OrderCreateDTO
        {
            Items = new List<OrderItemCreateDTO> { new OrderItemCreateDTO { BookId = bookId, Quantity = 1 } },
            CustomerId = Guid.NewGuid()
        };

        _repo.Setup(r => r.GetCustomerByIdAsync(dto.CustomerId)).ReturnsAsync(new Customer { IsDeleted = false });
        _repo.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync((Book)null);

        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddAsync(dto, _user));
    }

    [Test]
    public void AddAsync_ShouldThrow_WhenBookQuantityInsufficient()
    {
        var bookId = Guid.NewGuid();
        var dto = new OrderCreateDTO
        {
            Items = new List<OrderItemCreateDTO> { new OrderItemCreateDTO { BookId = bookId, Quantity = 5 } },
            CustomerId = Guid.NewGuid()
        };

        _repo.Setup(r => r.GetCustomerByIdAsync(dto.CustomerId)).ReturnsAsync(new Customer { IsDeleted = false });
        _repo.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(new Book { Id = bookId, Quantity = 1, Price = 100 });

        Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(dto, _user));
    }

    [Test]
    public async Task AddAsync_ShouldApplyPromotion_AndReturnTrue()
    {
        var bookId = Guid.NewGuid();
        var promoId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var dto = new OrderCreateDTO
        {
            Items = new List<OrderItemCreateDTO> { new OrderItemCreateDTO { BookId = bookId, Quantity = 2 } },
            CustomerId = customerId,
            PromotionId = promoId
        };

        _repo.Setup(r => r.GetCustomerByIdAsync(customerId)).ReturnsAsync(new Customer { IsDeleted = false });
        _repo.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(new Book { Id = bookId, Quantity = 10, Price = 100 });
        _repo.Setup(r => r.GetPromotionByIdAsync(promoId)).ReturnsAsync(new Promotion
        {
            Quantity = 1,
            Condition = 100,
            DiscountPercent = 0.1m,
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now.AddDays(1)
        });
        _repo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _service.AddAsync(dto, _user);
        Assert.That(result, Is.True);
    }
}
