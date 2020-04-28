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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task SendMessageToAll(string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", message);
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
