using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AttendanceTaking.Infra.CosmosDB
{
    class CosmosDBAttendanceRepository: AttendanceRepository
    {
        private const string DatabaseName = "attendance";
        private const string AttendanceContainerName = "attendance";

        private readonly CosmosClient cosmosClient;
        private readonly Container container;
        private readonly ILogger logger;

        public CosmosDBAttendanceRepository(CosmosClient _cosmosClient, ILogger _logger)
        {
            cosmosClient = _cosmosClient;
            logger = _logger;
            container = cosmosClient.GetContainer(DatabaseName, AttendanceContainerName);
        }

        public Task<Attendance[]> FindAll(Attendance attendance)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(Attendance attendance)
        {
            try {
                await container.CreateItemAsync(attendance);
                return true;
            }
            catch (Exception e)
            {
                logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> Update(Attendance attendance)
        {
            try {
                await container.UpsertItemAsync(attendance);
                return true;
            }
            catch (Exception e)
            {
                logger.LogInformation(e.Message);
                return false;
            }
        }

        public Task<bool> Delete(Attendance attendance)
        {
            throw new NotImplementedException();
        }
    }
}
