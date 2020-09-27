using System.Threading.Tasks;

namespace AttendanceTaker
{
	public interface AttendanceRepository
	{
		public Task<bool> Create(string userId, Attendance attendance);
	}
}
