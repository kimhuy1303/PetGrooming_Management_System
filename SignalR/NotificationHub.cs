using Microsoft.AspNetCore.SignalR;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using System.Security.Claims;

namespace PetGrooming_Management_System.SignalR
{
    public class NotificationHub : Hub
    {
        //private readonly INotificationRepository _notificationRepository;
        //public NotificationHub(INotificationRepository notificationRepository)
        //{
        //    _notificationRepository = notificationRepository;
        //}
        
        public async Task SendNotification(string id, string message)
        {
            await Clients.User(id).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToEmployees(string message)
        {
            await Clients.Group("Employee").SendAsync("ReceiveNotification ", message);
        }

        public async Task SendNotificationToManagers(string message)
        {
            await Clients.Group("Manager").SendAsync("ReceiveNotification ", message);
        }

        public override async Task OnConnectedAsync()
        {
            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "Employee")
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
            }else if(userRole == "Manager")
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "Manager");
            }
            //var userId = Context.User?.FindFirst("UserId")?.Value;
            //if (!string.IsNullOrEmpty(userId)) 
            //{
            //    var pendingNotifications = await _notificationRepository.GetAnnouncementsUnread(int.Parse(userId));
            //    foreach (var noti in pendingNotifications)
            //    {
            //        await SendNotification(userId, noti.Annoucement!.Content!);
            //    }
            //}
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

    }
}
