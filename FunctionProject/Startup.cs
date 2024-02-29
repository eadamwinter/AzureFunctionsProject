using FunctionProject.Services;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionProject.Startup))]

namespace FunctionProject;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddTransient<IMessageValidator, MessageValidator>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

        builder.Services.AddLogging();
    }
}
