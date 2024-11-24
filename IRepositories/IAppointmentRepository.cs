using PetGrooming_Management_System.Configs.Constant;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAppointment(AppointmentRequest appointmentdto, int customerId);
        Task<AppointmentDetail> MakeAnAppointment(AppointmentRequest appointmentdto);
        Task UpdateAppointment(int appointmentId,AppointmentRequest appointmentdto);
        Task<AppointmentDetailResponse> ViewAppointmentDetail(int appointmentId);
        Task<IEnumerable<AppointmentResponse>> GetAllAppointments(int page, int size);
        Task<Appointment> GetAppointmentById(int id);
        Task<AppointmentDetail> CreateAppointmentDetail(int appointmentId, AppointmentDetailRequest appointmentdetaildto);
        Task AddServicesToAppointment(AppointmentDetail appointmentDetaildto, AppointmentServicesRequest appointmentservicesdto);
        Task ChangeStatusAppointment(Appointment appointment,string status);

        Task<IEnumerable<AppointmentDetailResponse>> GetAppointmentsByDate(DateTime date);
        Task<IEnumerable<AppointmentResponse>> GetCustomerHistoryAppointments(int customerId, int page, int size);
        Task ConfirmAppointment(Appointment appointment);
    }
}
