using Data.Models;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

const string AdminRoleName = "admin";
const string UserRoleName = "user";
const string ReadonlyRoleName = "readonly";
const string AdminPolicyName = "adminonly";
const string UserPolicyName = "useronly";
const string AdminUserPolicyName = "adminuseronly";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Employee>(opts =>
{
    opts.SignIn.RequireConfirmedAccount = true;
    opts.SignIn.RequireConfirmedEmail = true;
    opts.Password.RequireDigit = true;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequiredLength = 8;
    opts.Password.RequireUppercase = true;
    opts.Password.RequiredUniqueChars = 1;
    opts.Password.RequireLowercase = true;

})
    .AddUserManager<UserManager<Employee>>()
    .AddRoles<Role>()
    .AddRoleManager<RoleManager<Role>>()
    .AddSignInManager<SignInManager<Employee>>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<Employee, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(AdminPolicyName, policy => policy.RequireRole(AdminRoleName));
    opts.AddPolicy(UserPolicyName, policy => policy.RequireRole(UserRoleName));
    opts.AddPolicy(ReadonlyRoleName, policy => policy.RequireRole(ReadonlyRoleName));
    opts.AddPolicy(AdminUserPolicyName, policy => policy.RequireRole(new[] { AdminRoleName, UserRoleName }));
});

var app = builder.Build();

await app.SeedDatabase(true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
