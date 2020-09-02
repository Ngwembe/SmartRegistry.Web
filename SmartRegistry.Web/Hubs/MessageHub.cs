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
    public class MessageHub : Hub
    {
        private class MessageExchange
        {
            public string Message { get; set; }
            public string ReceivedAt { get; set; }
            public string Sender { get; set; }
        }

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task SendMessageToAll(string message, string sender)
        {
            MessageExchange broadcast = new MessageExchange()
            {
                Message = message,
                ReceivedAt = DateTime.Now.ToString("g"),
                Sender = sender
            };

            return Clients.All.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(broadcast));
        }

        public Task SendConfirmationMessageToAll(int scheduleId)
        {
            if (scheduleId <= 0)    //  Invalid ScheduleId
                return null;

            var schedule = _context.Schedule
                .Include(s => s.Subject)
                .FirstOrDefault(m => m.ScheduleId == scheduleId);

            if (schedule == null)            
                return null;

            return Clients.All.SendAsync("ReceiveConfirmationMessage", JsonConvert.SerializeObject(new { schedule, message = "The lecture has been confirmed!" }));
        }
    }
}
