using Dukkantek.Test.Application;
using Dukkantek.Test.Infrastructure;

#region Build Config file & Create Logger

var configurationBuilder = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile(
           $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
           optional: true);

configurationBuilder.AddEnvironmentVariables();

var _config = configurationBuilder.Build();

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(_config);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// db init
await app.Services.InitializeDatabasesAsync();


app.UseHttpsRedirection();
app.UseRouting();
app.UseExceptionMiddleware();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

//app.MapControllers();

app.Run();
