using examfinal.Models;
using examfinal.Repositories;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});

builder.Services.AddScoped<NpgsqlConnection>(serviceProvider =>
{
    var connectionString =
        serviceProvider.GetRequiredService<IConfiguration>()
            .GetConnectionString("pgconn");

    return new NpgsqlConnection(connectionString);
});

builder.Services.AddScoped<ExamRepository>();

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

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.Run();
