using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyNotesApp;
using MyNotesApp.Core.IRepository;
using MyNotesApp.Core.Repository;
using MyNotesApp.Data;
using MyNotesApp.Models;
using MyNotesApp.Utilites;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext")));

builder.Services.AddIdentity<AppUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IFileUploadService,FileUploadService>();
//builder.Services.AddTransient<IUserAccessor, UserAccessor>();
builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions
{
    CloseButton = true,
    PositionClass = ToastPositions.TopLeft,
    Rtl = true,
    PreventDuplicates = true,
    ProgressBar= true,
    Title = ""
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;

    options.Lockout.MaxFailedAccessAttempts = 2;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath= "/Account/AccessDenied";
    options.LogoutPath= "/Account/Logout";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan= TimeSpan.FromMinutes(5);
});


builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("rolecreation", policy => policy.RequireRole("Admin"));
});
var app = builder.Build();
DataSeeding();
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
    pattern: "{controller=Home}/{action=index}/{id?}");

STInitializer.seed(app);
app.Run();

void DataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        DbInitialize.Initialize();
    }
}