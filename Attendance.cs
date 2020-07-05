using System;
using System.ComponentModel;
using Newtonsoft.Json;


namespace AttendanceTaking
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Attendance
    {
        [JsonProperty("type")]
        public AttendanceType Type
        {
            get; set;
        }
        
        [JsonProperty("occurredAt")]
        public DateTimeOffset OccurredAt
        {
            get; set;
        }

        public string GetDateString()
        {
            return OccurredAt.DateTime.Date.ToShortDateString();
        }
    }
}
