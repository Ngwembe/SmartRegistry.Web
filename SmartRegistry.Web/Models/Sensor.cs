using System;

namespace SmartRegistry.Web.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public bool IsAssigned { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string  LastModifiedBy { get; set; }
    }
}
