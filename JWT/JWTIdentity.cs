using Microsoft.AspNetCore.Identity;

namespace sti_sys_backend.JWT;

public class JWTIdentity : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}