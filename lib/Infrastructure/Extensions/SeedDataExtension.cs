using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using static System.Console;

namespace Infrastructure.Extensions
{
    public static class SeedDataExtension
    {
        static ILogger? _logger;
        static IServiceScope _scope = default!;
        static ApplicationDbContext _context = default!;

        const string AdminRoleName = "admin";
        const string UserRoleName = "user";
        const string ReadonlyRoleName = "readonly";

        const string DefaultPassword = "V0lunteer$";

        const string DefaultAdminUserEmail = "defaultAdmin@aiminspections.com";
        const string DefaultUserEmail = "defaultUser@aiminspections.com";
        const string DefaultReadOnlyUserEmail = "defaultReadonly@aiminspections.com";

        readonly static Claim DefaultUserClaim = new(issuer: "System", type: "Role", value: "user", valueType: typeof(string).Name);
        readonly static Claim DefaultAdminClaim = new(issuer: "System", type: "Role", value: "admin", valueType: typeof(string).Name);
        readonly static Claim DefaultReadonlyClaim = new(issuer: "System", type: "Role", value: "readonly", valueType: typeof(string).Name);

        public static async Task<IApplicationBuilder> SeedDatabase(this IApplicationBuilder app, bool clearDataBase)
        {
            _scope = app.ApplicationServices
                .CreateScope();

            _logger = _scope
                .ServiceProvider
                .GetRequiredService<ILogger<ApplicationDbContext>>();

            _context = _scope
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var _roleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var _userStore = _scope.ServiceProvider.GetRequiredService<IUserStore<Employee>>();
            var _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
            var _emailStore = GetEmailStore();
            var _signInManager = _scope.ServiceProvider.GetRequiredService<SignInManager<Employee>>();

            if (_context is null)
            {
                WriteLine("Unable to resolve context");
                throw new NullReferenceException(nameof(ApplicationDbContext));
            }

            try
            {
                ///Step 1 (Optional)
                DeleteDatabase();

                ///Step 2 (Required)
                ApplyMigrations();

                ///Step 3 (Required)
                await AddDefaultRoles();

                ///Step 4 (Required)
                await AddDefaultUser();

                ///Step 5 (Required)
                await AddDefaultAdminUser();

                ///Step 6 (Required)
                await AddDefaultReadOnlyUser();             
            }
            catch (Exception ex)
            {
                WriteLine($"Migration Failed: {ex.Message}");
            }

            return app;

            ///Apply the latest migrations to ensure Database and EF Core will work
            void ApplyMigrations()
            {
                try
                {
                    _context.Database.Migrate();
                    _logger?.LogInformation("Database Migrations Applied");

                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("Database Migrations Applied");
                    ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("Database Migrations Failed");
                    _logger?.LogError(ex.Message);
                    ForegroundColor = ConsoleColor.White;
                }
            }

            ///Delete the current Database to start fresh
            void DeleteDatabase()
            {
                if (clearDataBase)
                {
                    var result = _context.Database.EnsureDeleted();
                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Database Deleted: {args}", args: result.ToYesNo());
                }
            }

            ///Adds default roles
            async Task AddDefaultRoles()
            {
                await _roleManager.CreateAsync(new Role(UserRoleName));
                await _roleManager.CreateAsync(new Role(AdminRoleName));
                await _roleManager.CreateAsync(new Role(ReadonlyRoleName));
            }

            ///Gets an instance of IUserEmailStore
            IUserEmailStore<Employee> GetEmailStore()
            {
                if (!_userManager.SupportsUserEmail)
                    throw new NotSupportedException("The default UI requires a user store with email support.");

                return (IUserEmailStore<Employee>)_userStore;
            }

            ///Adds a default User
            async Task AddDefaultUser()
            {
                try
                {
                    var user = Activator.CreateInstance<Employee>();
                    user.FirstName = "standard";
                    user.LastName = "user";
                    user.EmailConfirmed = true;
                    user.PhoneNumber = "999-999-9999";
                    user.PhoneNumberConfirmed = true;
                    user.LockoutEnabled = false;

                    await _userStore.SetUserNameAsync(user, DefaultUserEmail, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, DefaultUserEmail, CancellationToken.None);

                    var result = await _userManager.CreateAsync(user, DefaultPassword);

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Standard User creation {args}", args: result.Succeeded.ToSuccessFailure());

                    var roleResult = await _userManager.AddToRoleAsync(user, UserRoleName);
                    var claimResult = await _userManager.AddClaimAsync(user, DefaultUserClaim);
                    var userNameClaimResult = await _userManager.AddClaimAsync(user, new(issuer: "System", type: "UsersName", value: user.ToString(), valueType: typeof(string).Name));

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Standard User role assignment {args}", args: roleResult.Succeeded.ToSuccessFailure());

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Standard User Successfully Signed-Out");
                }
                catch (Exception ex)
                {
                    BackgroundColor = ConsoleColor.Red;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogError("Default Standard User Failed to be created: {args}", args: ex.Message);

                    throw new InvalidOperationException($"Can't create an instance of '{nameof(Employee)}'. " +
                        $"Ensure that '{nameof(Employee)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                        $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
                }
            }

            ///Adds a default Admin User
            async Task AddDefaultAdminUser()
            {
                try
                {
                    var user = Activator.CreateInstance<Employee>();
                    user.FirstName = "admin";
                    user.LastName = "user";
                    user.EmailConfirmed = true;
                    user.PhoneNumber = "999-999-9999";
                    user.PhoneNumberConfirmed = true;
                    user.LockoutEnabled = false;

                    await _userStore.SetUserNameAsync(user, DefaultAdminUserEmail, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, DefaultAdminUserEmail, CancellationToken.None);

                    var result = await _userManager.CreateAsync(user, DefaultPassword);

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Admin User creation {args}", args: result.Succeeded.ToSuccessFailure());

                    var roleResult = await _userManager.AddToRoleAsync(user, AdminRoleName);
                    var claimResult = await _userManager.AddClaimAsync(user, DefaultAdminClaim);
                    var userNameClaimResult = await _userManager.AddClaimAsync(user, new(issuer: "System", type: "UsersName", value: user.ToString(), valueType: typeof(string).Name));

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Admin User role assignment {args}", args: roleResult.Succeeded.ToSuccessFailure());

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Admin User Successfully Signed-Out");
                }
                catch (Exception ex)
                {
                    BackgroundColor = ConsoleColor.Red;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogError("Default Admin User Failed to be created: {args}", args: ex.Message);

                    throw new InvalidOperationException($"Can't create an instance of '{nameof(Employee)}'. " +
                        $"Ensure that '{nameof(Employee)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                        $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
                }
            }

            ///Adds a default Readonly User
            async Task AddDefaultReadOnlyUser()
            {
                try
                {
                    var user = Activator.CreateInstance<Employee>();
                    user.FirstName = "readonly";
                    user.LastName = "user";
                    user.EmailConfirmed = true;
                    user.PhoneNumber = "999-999-9999";
                    user.PhoneNumberConfirmed = true;
                    user.LockoutEnabled = false;

                    await _userStore.SetUserNameAsync(user, DefaultReadOnlyUserEmail, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, DefaultReadOnlyUserEmail, CancellationToken.None);

                    var result = await _userManager.CreateAsync(user, DefaultPassword);

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Readonly User creation {args}", args: result.Succeeded.ToSuccessFailure());

                    var roleResult = await _userManager.AddToRoleAsync(user, ReadonlyRoleName);
                    var claimResult = await _userManager.AddClaimAsync(user, DefaultReadonlyClaim);
                    var userNameClaimResult = await _userManager.AddClaimAsync(user, new(issuer: "System", type: "UsersName", value: user.ToString(), valueType: typeof(string).Name));

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Readonly User role assignment {args}", args: roleResult.Succeeded.ToSuccessFailure());

                    BackgroundColor = ConsoleColor.Blue;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogInformation("Default Readonly User Successfully Signed-Out");
                }
                catch (Exception ex)
                {
                    BackgroundColor = ConsoleColor.Red;
                    ForegroundColor = ConsoleColor.White;

                    _logger?.LogError("Default Readonly User Failed to be created: {args}", args: ex.Message);

                    throw new InvalidOperationException($"Can't create an instance of '{nameof(Employee)}'. " +
                        $"Ensure that '{nameof(Employee)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                        $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
                }
            }
        }
    }
}
