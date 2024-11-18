using Microsoft.EntityFrameworkCore;
using TaskApi.Service;
using TaskApi.Data;
using TaskApi.Repository;
using FluentValidation.AspNetCore;
using TaskApi.Filter;
using TaskApi.Extension;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:8888") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true; // Disable DataAnnotations validation if unnecessary
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
    .ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddFluentValidationServices();

//builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();


//builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//builder.Services.AddTransient<IValidator<Customer>, CustomerValidator>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<CustomerRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskApi v1");
    c.RoutePrefix = string.Empty; 
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowSpecificOrigin");


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

