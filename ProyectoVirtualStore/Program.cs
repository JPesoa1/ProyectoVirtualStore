using Microsoft.EntityFrameworkCore;
using ProyectoVirtualStore.Data;
using ProyectoVirtualStore.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(30));

builder.Services.AddControllersWithViews();


string connectionString =
  "Data Source=LOCALHOST\\DESARROLLO;Initial Catalog=TIENDAVIRTUAL;User ID=SA;Password=MCSD2023";



builder.Services.AddTransient<IRepository, RepositorySQLTienda>();


builder.Services.AddDbContext<TiendaContext>
    (option => option.UseSqlServer(connectionString));
//builder.Services.AddDbContext<TiendaContext>
//    (option =>

//        option.UseOracle(connectionString)
//    );

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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Managed}/{action=Login}/{id?}");

app.Run();
