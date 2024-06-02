
using ChatServer.Extentions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "Server=localhost;Port=5432;User Id=postgres;Password=postgres");
}

builder.Services.AddCustomServices();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
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
var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
app.UseWebSockets(wsOptions);
app.UseCors("CorsPolicy");
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
