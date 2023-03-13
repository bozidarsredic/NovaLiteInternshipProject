using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Reflection;
using ToDo.Infrastructure;
using ToDoApi.Infrastructure;
using ToDoApi.Mapping;
using ToDoApi.Repositories;
using ToDoApi.Security;
using ToDoCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection(ApiKeyOptions.ApiKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddDbContext<ToDoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoDataBase")));

builder.Services.AddOpenApiDocument(options => options.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

//builder.Services.AddHostedService<RemainderService>();

builder.Services.AddScoped<IRepository<Note>, NoteRepository>();
builder.Services.AddScoped<IRepository<ToDoList>, ToDoListRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "cors", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});



var app = builder.Build();

app.UseCors("cors");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
context.Database.Migrate();

app.Run();







