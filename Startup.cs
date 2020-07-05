using System.Configuration;
using AttendanceTaking.Infra.CosmosDB;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AttendanceTaking.Startup))]
namespace AttendanceTaking
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((service) =>
           {
               var connectionString = ConfigurationManager.ConnectionStrings["CosmosDB"].ConnectionString;
               var cosmosClientBuilder = new CosmosClientBuilder(connectionString);
               return cosmosClientBuilder.Build();
           });
           builder.Services.AddLogging();
           builder.Services.AddSingleton<AttendanceRepository, CosmosDBAttendanceRepository>();
        }
    }
}
