using AutonomyForum.Extentions;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.ConfigureApplicationSettings();

builder.ConfigureContainer();
builder.ConfigureDatabase(settings);
builder.ConfigureIdentity();
builder.ConfigureJwt(settings);
builder.ConfigureCors();
builder.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

//app.ConfigureCors();
app.ConfigureCookies();

app.UseCookieJwt();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();