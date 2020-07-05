using System.Threading.Tasks;

namespace AttendanceTaking
{
    public interface AttendanceRepository 
    {
        public Task<Attendance[]> FindAll(int year, int month);
        public Task<bool> Create(Attendance attendance);

        public Task<bool> Update(Attendance attendance);
        public Task<bool> Delete(Attendance attendance);

    }
}
