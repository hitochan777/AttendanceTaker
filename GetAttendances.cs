using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace AttendanceTaking
{
    public class GetAttendances
    {
        private readonly AttendanceRepository _attendanceRepository;
        public GetAttendances(AttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [FunctionName("GetAttendances")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            int year, month;
            var now = DateTimeOffset.UtcNow.AddHours(9);
            string yearString = req.Query["year"][0] ?? $"{now.Year}"; 
            string monthString = req.Query["month"][0] ?? $"{now.Month}";

            int.TryParse(yearString, out year);
            int.TryParse(monthString, out month);

            var attendances = await _attendanceRepository.FindAll(year, month);
            string responseMessage = JsonConvert.SerializeObject(attendances);
            return new OkObjectResult(responseMessage);
        }
    }
}
