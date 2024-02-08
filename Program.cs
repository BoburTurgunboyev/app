using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyProject.Domain.Models;
using MyProject.Infrastructure.Data;
using MyProject.Infrastructure.Repositories.AuditRepo;
using MyProject.Infrastructure.Repositories.ProductRepo;
using MyProject.Services.ExtentionFunctions;
using MyProject.Services.Services.AccountServices;
using MyProject.Services.Services.ProductServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<VatCalculator>();



// Add services to the container.
//Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();


//Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TeamProjectMVC API",
        Version = "v1"
    });
});

//for identity



builder.Services.AddHttpClient();

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>();


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
//app.Use(async (context, next) =>
//{
//    if (context.Request.Path.StartsWithSegments("/swagger"))
//    {
//        if (context.User.Identity.IsAuthenticated)
//        {
//            if (!context.User.IsInRole("ADMIN"))
//            {
//                context.Response.StatusCode = 403;
//                return;
//            }
//        }
//        else
//        {
//            context.Response.Redirect("/Account/Login");
//            return;
//        }
//    }
//    await next.Invoke();
//});
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();
