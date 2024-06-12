
using ChatServer.Extentions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "Server=localhost;Port=5432;User Id=postgres;Database=chat;Password=postgres;");
    Environment.SetEnvironmentVariable("DB_CONNECTION_STRING_MIGRATOR", "Host=localhost;Username=postgres;Password=postgres;Port=5432;Database=postgres;");
}

builder.Services.AddCustomServices();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
app.UseWebSockets(wsOptions);
app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
           name: "default",
           pattern: "{controller=Page}/{action=Index}/{id?}");
});
app.Run();
