using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.Models;
using PixelIt.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 30 * 1024 * 1024; // 30 MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = 30 * 1024 * 1024; // 30 MB
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<LikeService>();
builder.Services.AddScoped<FollowService>();
builder.Services.AddScoped<ImageCollectionService>();


builder
    .Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        //options.SignIn.RequireConfirmedAccount = builder
        //    .Configuration.GetSection("Identity")
        //    .GetValue<bool>("RequireConfirmedAccount");

        //options.Password.RequiredLength = builder
        //    .Configuration.GetSection("Identity")
        //    .GetValue<int>("RequiredLength");

        //options.Password.RequireDigit = builder
        //    .Configuration.GetSection("Identity")
        //    .GetValue<bool>("RequireDigit");

        //options.Password.RequireLowercase = builder
        //    .Configuration.GetSection("Identity")
        //    .GetValue<bool>("RequireLowercase");

        //options.Password.RequireNonAlphanumeric = builder
        //    .Configuration.GetSection("Identity")
        //    .GetValue<bool>("RequireNonAlphanumeric");

        options.Password.RequireUppercase = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireUppercase");

        //options.Password.RequiredUniqueChars = builder
        //    .Configuration.GetSection("Identity")
        //    .GetValue<int>("RequireUniqueChars");
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddScoped<RoleService>();
//builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.UseCors("AllowAll");
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
