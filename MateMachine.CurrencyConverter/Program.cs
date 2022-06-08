using MateMachine.CurrencyConverter.Business;
using MateMachine.CurrencyConverter.Data;
using MateMachine.CurrencyConverter.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
DbStartupHelper.AddDbContextTransient(builder.Services);
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
// This service cannot be singleton for now, check!
builder.Services.AddTransient<ICurrencyConverter, CurrencyConverter>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
