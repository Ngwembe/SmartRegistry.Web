using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartRegistry.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRegistry.Web.Hubs
{
    public interface INotificationHub
    {
        Task BoardNotification(string message, string sender);
    }

    public class NotificationHub : Hub<INotificationHub>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void SendMessageToAll(string message, string sender)
        {
            MessageExchange broadcast = new MessageExchange()
            {
                Message = message,
                ReceivedAt = DateTime.Now.ToString("g"),
                Sender = sender
            };

            Clients.All.BoardNotification("OnlineUserMessage", JsonConvert.SerializeObject(broadcast));
        }

    }

    public class MessageExchange
    {
        public string Message { get; set; }
        public string ReceivedAt { get; set; }
        public string Sender { get; set; }
    }
}
