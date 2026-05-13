using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteMonitor.UI.Models
{
    public class AddDeviceCommand
    {
        [Required]
        [StringLength(36, ErrorMessage = "Identifier too long (32 character limit).")]
        public string Id { get; set; }
       
        [Required]
        [StringLength(32, ErrorMessage = "Name too long (16 character limit).")]
        public string Name { get; set; }
    }

    public class DeleteDeviceCommand
    {
        [Required]
        [StringLength(36, ErrorMessage = "Identifier too long (32 character limit).")]
        public string Id { get; set; }
    }
}
