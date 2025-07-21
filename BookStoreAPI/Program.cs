using BookStoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

using BookStoreAPI.Services.AuthService.Interfaces;
using BookStoreAPI.Services.AuthService;
using BookStoreAPI.Services.AuthService.Repositories;
using BookStoreAPI.Services.EmailService;

using BookStoreAPI.Services.CategoryService.Interfaces;
using BookStoreAPI.Services.CategoryService;
using BookStoreAPI.Services.CategoryService.Repositories;

using BookStoreAPI.Services.AuthorService.Interfaces;
using BookStoreAPI.Services.AuthorService;
using BookStoreAPI.Services.AuthorService.Repositories;

using BookStoreAPI.Services.BookService.Interfaces;
using BookStoreAPI.Services.BookService;
using BookStoreAPI.Services.BookService.Repositories;

using BookStoreAPI.Services.CustomerSevice.Interfaces;
using BookStoreAPI.Services.CustomerSevice;
using BookStoreAPI.Services.CustomerSevice.Repositories;

using BookStoreAPI.Services.StaffService.Interfaces;
using BookStoreAPI.Services.StaffService;
using BookStoreAPI.Services.StaffService.Repositories;

using BookStoreAPI.Services.PublisherService.Interfaces;
using BookStoreAPI.Services.PublisherService;
using BookStoreAPI.Services.PublisherService.Repositories;

using BookStoreAPI.Services.PromotionService.Interfaces;
using BookStoreAPI.Services.PromotionService;
using BookStoreAPI.Services.PromotionService.Repositories;

using BookStoreAPI.Services.OrderService.Interfaces;
using BookStoreAPI.Services.OrderService;
using BookStoreAPI.Services.OrderService.Repositories;

using BookStoreAPI.Middlewares;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
});

// Đọc biến môi trường
builder.Configuration.AddEnvironmentVariables();
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? builder.Configuration["Jwt:Key"];
var smtpUser = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? builder.Configuration["EmailSettings:SmtpUsername"];
var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? builder.Configuration["EmailSettings:SmtpPassword"];

// Kết nối DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// CORS cho DevX front-end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Swagger (Dev only)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. Example: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Services & DI
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<EmailSenderService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();

builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Helpers
builder.Services.AddHttpContextAccessor();

// Controllers
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Dev: Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API V1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
