using FluentValidation;
using FluentValidation.AspNetCore;
using System.Runtime.CompilerServices;
using TaskApi.Validator;

namespace TaskApi.Extension
{
    public static class FluentValidationExtension
    {
        public static void AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

            services.AddFluentValidationAutoValidation();
        }
    }
}
