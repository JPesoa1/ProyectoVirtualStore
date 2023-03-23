using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ProyectoVirtualStore.Data;
using ProyectoVirtualStore.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(30));
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();


builder.Services.AddControllersWithViews(x => x.EnableEndpointRouting = false).AddSessionStateTempDataProvider();


//string connectionString =
//  "Data Source=LOCALHOST\\DESARROLLO;Initial Catalog=TIENDAVIRTUAL;User ID=SA;Password=MCSD2023";

string connectionString = builder.Configuration.GetConnectionString("SqlAzureTiendaVirtual");

builder.Services.AddTransient<IRepository, RepositorySQLTienda>();
builder.Services.AddDbContext<TiendaContext>
    ( option => option.UseSqlServer(connectionString) 
    );




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

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}"
            );
    routes.MapRoute(
           name: "game",
           template: "{controller=Games}/{action=Index}/{idjuego?}"
           );


});


app.Run();
