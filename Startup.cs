using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AttendanceTaker.Startup))]

namespace AttendanceTaker
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddSingleton((service) =>
			{
				var connectionString = GetEnvironmentVariable("ConnectionStrings:CosmosDB");
				var cosmosClientBuilder = new CosmosClientBuilder(connectionString);
				return cosmosClientBuilder.Build();
			});
			builder.Services.AddLogging();
			builder.Services.AddSingleton<AttendanceRepository, CosmosDBAttendanceRepository>();
		}

		private static string GetEnvironmentVariable(string name)
		{
			return System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Process);
		}
	}
}
