using FinalProject.Models;
using FinalProject.Repository.CartRepository;
using FinalProject.Repository.OrderRepository;
using FinalProject.Repository.ProductRepository;
using FinalProject.Repository.UserRepository;
using FinalProject.Service.IService;
using FinalProject.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IProduct, ProductRepository>();
builder.Services.AddScoped<ICart, CartRepository>();
builder.Services.AddScoped<IOrder, OrderRepository>();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();  


builder.Services.AddDbContext<FinalDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyCon")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(options =>
{

options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };

});
/*configure authorization for your application*/
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
//This allows any origin to access the API.
.AllowAnyOrigin()
//This allows any HTTP header to be sent with requests to the API.
.AllowAnyHeader()
//This allows any HTTP method to be used with requests to the API.
.AllowAnyMethod());


// redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

app.UseAuthorization();

//map incoming requests to controller actions.
app.MapControllers();

app.Run();
