using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AttendanceTaker
{
	public class ReceiveDeviceMessage
	{
		private readonly AttendanceRepository _attendanceRepository;
		public ReceiveDeviceMessage(AttendanceRepository attendanceRepository)
		{
			_attendanceRepository = attendanceRepository;
		}
		[FunctionName("ReceiveDeviceMessage")]
		public async Task Run([IoTHubTrigger("messages/events", Connection = "IoTHubTriggerConnection")] EventData message, ILogger log)
		{
			var requestBody = Encoding.UTF8.GetString(message.Body.Array);
			var attendance = JsonConvert.DeserializeObject<Attendance>(requestBody);
			await _attendanceRepository.Create(attendance.UserId, attendance);
		}
	}
}
