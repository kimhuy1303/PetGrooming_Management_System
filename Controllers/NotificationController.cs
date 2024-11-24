using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult> GetAllAnnouncementsByUserId(int id)
        {
            var res = await _notificationRepository.GetListByUserId(id);
            if (res.IsNullOrEmpty()) return BadRequest("User has no annoucements");
            return Ok(res);
        }
    }
}
