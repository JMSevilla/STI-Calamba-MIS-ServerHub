using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("device_recognition")]
public class DeviceRecognition : IDeviceRecognition
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public string deviceKey { get; set; }
    public int accountId { get; set; }
    public int signinRequest { get; set; }
    public int rejects { get; set; }
    public int approved { get; set; }
    public AppGranted _appGranted { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}