using EcredoWebApp.Components;
using EcredoWebApp.Data;
using EcredoWebApp.Models;
using EcredoWebApp.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// =========================================================
// SERVICES
// =========================================================

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;

        // Allow our seeded administrator to log in.
        options.SignIn.RequireConfirmedAccount = false;

        // Account lockout settings.
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// =========================================================
// APPLICATION
// =========================================================

var app = builder.Build();


// =========================================================
// SEED IDENTITY
// =========================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(
        services,
        app.Configuration);
}


// =========================================================
// MIDDLEWARE
// =========================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();


// =========================================================
// LOGIN ENDPOINT
// =========================================================

app.MapPost(
    "/account/login",
    async (
        HttpContext httpContext,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager) =>
    {
        var form = await httpContext.Request.ReadFormAsync();

        var email = form["Email"]
            .ToString()
            .Trim();

        var password = form["Password"]
            .ToString();

        var rememberMe =
            form["RememberMe"] == "true";


        // -----------------------------------------------------
        // BASIC VALIDATION
        // -----------------------------------------------------

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return Results.Redirect(
                "/login?error=invalid");
        }


        // -----------------------------------------------------
        // FIND USER
        // -----------------------------------------------------

        var user =
            await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Results.Redirect(
                "/login?error=invalid");
        }


        // -----------------------------------------------------
        // CHECK ACCOUNT STATUS
        // -----------------------------------------------------

        if (!user.IsActive)
        {
            return Results.Redirect(
                "/login?error=inactive");
        }


        // -----------------------------------------------------
        // SIGN IN
        // -----------------------------------------------------

        var result =
            await signInManager.PasswordSignInAsync(
                user,
                password,
                rememberMe,
                lockoutOnFailure: true);


        // -----------------------------------------------------
        // RESULT
        // -----------------------------------------------------

        if (result.Succeeded)
        {
            var roles =
                await userManager.GetRolesAsync(user);


            // ADMIN
            if (roles.Contains(
                IdentitySeeder.AdminRole))
            {
                return Results.Redirect(
                    "/dashboard");
            }


            // CUSTOMER
            if (roles.Contains(
                IdentitySeeder.CustomerRole))
            {
                return Results.Redirect(
                    "/store");
            }


            // User has no recognised application role.
            await signInManager.SignOutAsync();

            return Results.Redirect(
                "/login?error=general");
        }


        if (result.IsLockedOut)
        {
            return Results.Redirect(
                "/login?error=locked");
        }


        if (result.IsNotAllowed)
        {
            return Results.Redirect(
                "/login?error=notallowed");
        }


        return Results.Redirect(
            "/login?error=invalid");
    });


// =========================================================
// LOGOUT ENDPOINT
// =========================================================

app.MapPost(
    "/account/logout",
    async (
        SignInManager<ApplicationUser> signInManager) =>
    {
        await signInManager.SignOutAsync();

        return Results.Redirect("/");
    });


// =========================================================
// BLAZOR
// =========================================================

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();