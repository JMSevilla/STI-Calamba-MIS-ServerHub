using System.Security.Cryptography;

namespace sti_sys_backend.Utilization;

public class JwtJitsiServe
{
    public string userName { get; set; }
    public string userEmail { get; set; }
    public string? roomName { get; set; }
    public int userId { get; set; }
}