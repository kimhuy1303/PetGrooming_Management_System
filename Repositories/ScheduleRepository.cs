using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.Services;

namespace PetGrooming_Management_System.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly MainDBContext _dbcontext;
        private readonly ScheduleService _scheduleService;
        private readonly IEmployeeRepository _employeeRepository;
        private  readonly IEmployeeShiftRepository _employeeShiftRepository;
        private readonly IShiftRepository _shiftRepository;
       
        public ScheduleRepository(ScheduleService scheduleService, MainDBContext mainDBContext, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository, IEmployeeShiftRepository employeeShiftRepository)
        {
            _scheduleService = scheduleService;
            _dbcontext = mainDBContext;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _employeeShiftRepository = employeeShiftRepository;
        }

        public async Task UpdateEmloyeeShiftInSchedule(int scheduleId, EmployeeShiftRequest employeeshiftdto)
        {
            var employeeshift = await _dbcontext.EmployeeShifts.FirstOrDefaultAsync(e => e.EmployeeId == employeeshiftdto.EmployeeId && e.Date.Date == employeeshiftdto.Date.Date && e.ShiftId == employeeshiftdto.ShiftId);
            if (employeeshift != null) 
            {
                employeeshift.ScheduleId = scheduleId;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task CreateSchedule(ScheduleRequest rawSchedule)
        {
            var schedule = new Schedule
            {
                startDate = rawSchedule.StartDate,
                endDate = rawSchedule.EndDate,
                //EmployeeShifts = rawSchedule.EmployeeShifts
            };
            await _dbcontext.Schedules.AddAsync(schedule);
            foreach (var shift in rawSchedule.EmployeeShifts)
            {
                // Kiểm tra xem shift có tồn tại không
                var trackedShift = await _dbcontext.EmployeeShifts
                    .FirstOrDefaultAsync(e => e.EmployeeId == shift.EmployeeId
                                           && e.ShiftId == shift.ShiftId
                                           && e.Date == shift.Date);
                var employee = await _employeeRepository.GetEmployeeById((int)shift.EmployeeId);
                var workHours = await _shiftRepository.GetWorkHoursInTimeSlot((int)shift.ShiftId);
                await _employeeShiftRepository.UpdateTotalHoursWorkOfEmployee(workHours, employee);
                if (trackedShift != null)
                    // Gán shift đã tồn tại vào Schedule
                    schedule.EmployeeShifts.Add(trackedShift);
                else
                    // Gán shift mới nếu chưa tồn tại
                    schedule.EmployeeShifts.Add(shift);

            }
            
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<ScheduleRequest> RawSchedule(DateTime start, DateTime end)
        {
            var schedule = new ScheduleRequest
            {
                StartDate = start,
                EndDate = end,
                EmployeeShifts = await _scheduleService.GenerateSchedule(start, end)
            };
            return schedule;
        }

        //public async Task<List<EmployeeShift>> GetListEmployeeShiftInSchedule(int scheduleId)
        //{
        //    var listEmployeeShifts = await _dbcontext.EmployeeShifts.Where(e => e.ScheduleId == scheduleId)
        //                                                            .Include(e => e.Employee)
        //                                                            .Include(e => e.Shift)
        //                                                            .ToListAsync();
        //    return listEmployeeShifts;                                                   
        //}

        public async Task<Schedule> GetScheduleById(int scheduleId)
        {
            return await _dbcontext.Schedules.FirstOrDefaultAsync(e => e.Id == scheduleId);
        }

        public async Task<ScheduleResponse> GetScheduleByWeek(DateTime start, DateTime end)
        {
            //var res = await _dbcontext.Schedules.Include(e => e.EmployeeShifts).FirstOrDefaultAsync(e => e.startDate.Date >= start.Date && e.endDate.Date <= end.Date);
            //if (res != null) res.EmployeeShifts = res.EmployeeShifts.OrderBy(s => s.Date).ToList();
            //return new ScheduleResponse
            //{
            //    ScheduleId = res.Id,

            //};
            return await _dbcontext.Schedules.Where(e => e.startDate.Date >= start.Date && e.endDate.Date <= end.Date).Select(e => new ScheduleResponse
            {
                ScheduleId = e.Id,
                EmployeeShifts = e.EmployeeShifts.Select(e => new EmployeeShiftResponse
                {
                    ShiftId = e.ShiftId,
                    TimeSlot = e.Shift.TimeSlot,
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.Employee.FullName,
                    Date = e.Date.Date
                }).OrderBy(e => e.Date).ToList()
            }).FirstOrDefaultAsync();
        }

        public async Task RemoveEmployeeShift(int scheduleId, EmployeeShiftRequest employeeshiftdto)
        {
            var employeeShift = await GetEmployeeShiftInSchedule(scheduleId, employeeshiftdto);
            if(employeeShift != null) {
                employeeShift.Schedule = null;
                employeeShift.ScheduleId = null;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<EmployeeShift> GetEmployeeShiftInSchedule(int scheduleId, EmployeeShiftRequest employeeshiftdto)
        {
            return await _dbcontext.EmployeeShifts.FirstOrDefaultAsync(e => e.ScheduleId == scheduleId && e.EmployeeId == employeeshiftdto.EmployeeId && e.Date.Date == employeeshiftdto.Date.Date && e.ShiftId == employeeshiftdto.ShiftId);
        }

        public async Task<IEnumerable<EmployeeShiftResponse>> GetEmployeeShiftsByEmployee(int employeeId, ScheduleResponse schedule)
        {

            var result = schedule.EmployeeShifts.Where(e => e.EmployeeId == employeeId).Select(shift => new EmployeeShiftResponse
            {
                EmployeeId = shift.EmployeeId,
                TimeSlot = shift.TimeSlot,
                EmployeeName = shift.EmployeeName,
                ShiftId = shift.ShiftId,
                Date = shift.Date,
            }).ToList();
            return result;
        }

       
    }
} 
