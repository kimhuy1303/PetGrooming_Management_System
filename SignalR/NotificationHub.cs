using Microsoft.AspNetCore.SignalR;
using PetGrooming_Management_System.DTOs.Requests;
using System.Security.Claims;

namespace PetGrooming_Management_System.SignalR
{
    public class NotificationHub : Hub
    {
        
        public async Task SendNotification(string id, string message)
        {
            await Clients.User(id).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToEmployees(string message)
        {
            await Clients.Group("Employee").SendAsync("ReceiveNotification ", message);
        }


        public override Task OnConnectedAsync()
        {
            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "Employee")
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

    }
}
