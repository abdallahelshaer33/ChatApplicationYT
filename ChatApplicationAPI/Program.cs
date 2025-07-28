using ChatApplicationAPI;
using ChatApplicationAPI.Hub;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(opt =>{
    opt.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://localhost:4200")
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowCredentials();
    });
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string ,UserRoomConnection>>(opt => 
new Dictionary<string,UserRoomConnection>());

var app = builder.Build();

   
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors();
app.UseEndpoints(end =>
{
    end.MapHub<ChatHub>("/chat");
});
app.MapControllers();




app.Run();

