using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AttendanceTaking
{
    public class LogAttendance
    {
        private readonly AttendanceRepository _attendanceRepository;
        public LogAttendance(AttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [FunctionName("LogAttendance")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", "put", Route = "{userId}")] HttpRequest req,
            ILogger log, string userId)
        { 
            if (String.IsNullOrEmpty(userId))
            {
                return new BadRequestResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Raw request: {requestBody}");
            var attendance = JsonConvert.DeserializeObject<Attendance>(requestBody, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            attendance.OccurredAt = DateTimeOffset.UtcNow;

            switch (req.Method)
            {
                case "POST":
                    {
                        var ok = await _attendanceRepository.Create(userId, attendance);
                        if (!ok)
                        {
                            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        }
                        break;
                    }
                case "PUT":
                    {
                        var ok = await _attendanceRepository.Update(userId, attendance);
                        if (!ok)
                        {
                            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        }
                        break;
                    }
                default:
                    return new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
            }

            return new StatusCodeResult(StatusCodes.Status200OK);
        }
    }
}
