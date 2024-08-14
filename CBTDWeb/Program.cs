using DataAccess;
using DataAccess.DbInitializer;

// using DataAcess
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Utility;

//convert a console app to web application
var builder = WebApplication.CreateBuilder(args);

//meaning of these service

// Add services to the container.
// building pipeline from backend to the front end
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// 1. create a dataservice and pipeline, dbcontext, create connection to the database
// this part is changed when change database type
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // use sql server
//2. in case user cannot create, display error to user
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//3. Permissino
//remove the default identity
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
//    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
	.AddEntityFrameworkStores<ApplicationDbContext>();

//initialize the database service
builder.Services.AddScoped<DbInitializer>();

builder.Services.AddScoped<UnitOfWork>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddSingleton<IEmailSender, EmailSender>();

//4. Enhance html with adding C# function
builder.Services.AddRazorPages();

//5. Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

SeedDatabase(); 
//
app.Run();


// will call the inilizer, to inilize
void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    dbInitializer.Initialize();
}

