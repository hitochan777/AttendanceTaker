using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.Cosmos.Linq;
using System.Collections.Generic;

namespace AttendanceTaking.Infra.CosmosDB
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

        public async Task<Attendance[]> FindAll(int year, int month)
        {
            var result = new List<Attendance>();
            var start = new DateTimeOffset(new DateTime(year, month, 1), new TimeSpan(9, 0, 0));
            var end = new DateTimeOffset(new DateTime(year, month, 1), new TimeSpan(9, 0, 0)).AddMonths(1);
            var iterator = container.GetItemLinqQueryable<Attendance>().Where(attendance => attendance.OccurredAt >= start && attendance.OccurredAt < end).ToFeedIterator();
            foreach (var item in await iterator.ReadNextAsync())
            {
                result.Add(item);
            }
            return result.ToArray();

        }

        public async Task<bool> Create(Attendance attendance)
        {
            return await Update(attendance);
        }

        public async Task<bool> Update(Attendance attendance)
        {
            try
            {
                await container.UpsertItemAsync(new
                {
                    id = $"hitochan-{attendance.Type}-{attendance.OccurredAt}",
                    userId = "hitochan",
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

        public Task<bool> Delete(Attendance attendance)
        {
            throw new NotImplementedException();
        }
    }
}
