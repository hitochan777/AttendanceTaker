using System;
using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AttendanceTaker.Startup))]

namespace AttendanceTaker
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddSingleton<HttpClient>(sp => new HttpClient
			{
				BaseAddress = new Uri("BASE_URL"),
				DefaultRequestHeaders =
					{
						{"x-functions-key", "FUNCTIONS_KEY_HERE"}
					}
			}
			);
		}
	}
}
