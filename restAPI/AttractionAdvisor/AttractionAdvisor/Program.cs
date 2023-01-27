using AttractionAdvisor.DataAccess;
using AttractionAdvisor.Interfaces;
using AttractionAdvisor.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<AttractionAdvisorDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("AttractionAdvisorConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCors", policy => 
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Content-Type")
            .WithExposedHeaders("Access-Control-Allow-Origin")
            .WithExposedHeaders("Access-Control-Allow-Headers")
            .WithExposedHeaders("Access-Control-Allow-Methods")
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ApiCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
