using CoursAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>

{

    options.UseSqlite("Data Source=app.db");

});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "V1",
        Title = "Todo API",
        Description = "A simple example ASP.NET Core Web API",
        TermsOfService = new Uri("https://google.com"),
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "Thomas BDC",
            Email = "mail@mail.com",
            Url = new Uri("https://google.com")
        }
    });
});


var app = builder.Build();//----------------------------------------------------------------------------------------------------



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();


app.UseDefaultFiles();
app.UseStaticFiles();



app.Run();
