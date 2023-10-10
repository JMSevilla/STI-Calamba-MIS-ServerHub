using Microsoft.AspNetCore.Identity;

namespace sti_sys_backend.JWT;

public class Auth_helper : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}