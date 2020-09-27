using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace AttendanceTaker
{
	class CosmosDBAttendanceRepository : AttendanceRepository
	{
		private const string DatabaseName = "attendance";
		private const string AttendanceContainerName = "attendance";

		private readonly CosmosClient cosmosClient;
		private readonly Container container;

		public CosmosDBAttendanceRepository(CosmosClient _cosmosClient)
		{
			cosmosClient = _cosmosClient;
			container = cosmosClient.GetContainer(DatabaseName, AttendanceContainerName);
		}

		public async Task<bool> Create(string userId, Attendance attendance)
		{
			try
			{
				await container.UpsertItemAsync(new
				{
					id = generateId(),
					userId,
					occurredAt = attendance.OccurredAt,
					type = attendance.Type,
				});
				return true;
			}
			catch
			{
				return false;
			}
		}

		private string generateId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}
