using AutonomyForum.Extentions;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.ConfigureApplicationSettings();

builder.ConfigureContainer();
builder.ConfigureDatabase(settings);
builder.ConfigureIdentity();
builder.ConfigureJwt(settings);
builder.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.ConfigureCors();
app.ConfigureCookies();

app.UseJwtCookie();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

app.Run();