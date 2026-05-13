using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace RemoteMonitor.UI.Models
{
    public class UpdateDeviceConfigurationCommand
    {
        public string Id { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Identifier too long (16 character limit).")]
        public string Name { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "TemperatureOffset too long (3 character limit).")]
        public string TemperatureOffset { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "HumidityOffset too long (3 character limit).")]
        public string HumidityOffset { get; set; }
    }
}
