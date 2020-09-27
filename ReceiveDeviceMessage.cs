using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AttendanceTaker
{
	public class ReceiveDeviceMessage
	{
		private static readonly string ENDPOINT_PATH = "/api/LogAttendance";

		public ReceiveDeviceMessage(HttpClient httpClient)
		{
			this.client = httpClient;
		}
		[FunctionName("ReceiveDeviceMessage")]
		public async Task Run([IoTHubTrigger("messages/events", Connection = "IoTHubTriggerConnection")] EventData message, ILogger log)
		{
			var requestBody = Encoding.UTF8.GetString(message.Body.Array);
			var data = new StringContent(requestBody, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(ENDPOINT_PATH, data);
			var result = response.Content.ReadAsStringAsync().Result;
		}
	}
}
