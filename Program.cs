using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Repos.Classes;
using ms_evva_core.Services.Interfaces;
using ms_evva_core.Services.Classes;
using ms_evva_core.Utils;
using ms_evva_core.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationHelper.Initialize(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddSignalR();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Repositories
builder.Services.AddScoped<IHostRepository, HostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IDockerConfigRepository, DockerConfigRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IProjectWorkflowRepository, ProjectWorkflowRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<IHostServiceRepository, HostServiceRepository>();
builder.Services.AddScoped<IHostMetricRepository, HostMetricRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IDeploymentRepository, DeploymentRepository>();
// Services
builder.Services.AddScoped<IHostControllerService, HostControllerService>();
builder.Services.AddScoped<IUserControllerService, UserControllerService>();
builder.Services.AddScoped<ITokenControllerService, TokenControllerService>();
builder.Services.AddScoped<IDockerConfigControllerService, DockerConfigControllerService>();
builder.Services.AddScoped<IProjectControllerService, ProjectControllerService>();
builder.Services.AddScoped<IWorkflowControllerService, WorkflowControllerService>();
builder.Services.AddScoped<IProjectWorkflowControllerService, ProjectWorkflowControllerService>();
builder.Services.AddScoped<IRepositoryControllerService, RepositoryControllerService>();
builder.Services.AddScoped<IHostServiceControllerService, HostServiceControllerService>();
builder.Services.AddScoped<IHostMetricControllerService, HostMetricControllerService>();
builder.Services.AddScoped<IAuditLogControllerService, AuditLogControllerService>();
builder.Services.AddScoped<IDeploymentControllerService, DeploymentControllerService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection(); // Disabled for development to avoid CORS issues with HTTP frontend.
app.UseRouting();
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();
app.MapHub<HostHub>("/hostHub");

app.Run();
