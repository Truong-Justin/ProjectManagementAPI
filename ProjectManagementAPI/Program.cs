using ProjectManagementAPI.Repositories;
using ProjectManagementAPI.Repositories.BugRepository;
using ProjectManagementAPI.Repositories.EmployeeRepository;
using ProjectManagementAPI.Repositories.ProjectManagerRepository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registers transient services for dependency-injection
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IBugRepository, BugRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IProjectManagerRepository, ProjectManagerRepository>();

builder.Configuration.GetConnectionString("SQLCONNSTR_CONNECTION");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

