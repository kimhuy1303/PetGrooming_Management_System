using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.IRepositories;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet("/Announcements/{id}")]
        public async Task<ActionResult> GetAllAnnouncementsByUserId(int userId)
        {
            var res = await _notificationRepository.GetListByUserId(userId);
            if (res.IsNullOrEmpty()) return BadRequest("User has no annoucements");
            return Ok(res);
        }
    }
}
