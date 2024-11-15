using PetGrooming_Management_System.Configs.Constant;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAppointment(AppointmentRequest appointmentdto, int customerId);
        Task<AppointmentDetail> MakeAnAppointment(int customerId, AppointmentRequest appointmentdto);
        Task UpdateAppointment(int appointmentId, AppointmentRequest appointmentdto);

        Task<object> ViewAppointmentDetail(int appointmentId);
        Task CancelAppointment(int appointmentId);

        Task<IEnumerable<Appointment>> GetAllAppointments();
        Task<Appointment> GetAppointmentById(int id);
        Task<AppointmentDetail> CreateAppointmentDetail(int appointmentId, AppointmentDetailRequest appointmentdetaildto);
        Task AddServicesToAppointment(int appointmentDetailId, AppointmentServicesRequest appointmentservicesdto);
        Task ChangeStatusAppointment(Appointment appointment,Status status);
    }
}
