using Microsoft.AspNetCore.Identity;
using ShoeStoreASP.Models.Domain;
using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Service.Abstract;
using ShoeStoreASP.Service.Implementation;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

var cloudinaryConfig = builder.Configuration.GetSection("CloudinarySettings");
var cloudinaryAccount = new Account(
    cloudinaryConfig["CloudName"],
    cloudinaryConfig["ApiKey"],
    cloudinaryConfig["ApiSecret"]
);

var cloudinary = new Cloudinary(cloudinaryAccount);

builder.Services.AddSingleton(cloudinary);

builder.Services.AddScoped<IUserAuthenticate, UserAuthenticate>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IShoeService, ShoeService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
