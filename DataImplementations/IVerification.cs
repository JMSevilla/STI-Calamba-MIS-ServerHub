using System.Numerics;

namespace sti_sys_backend.DataImplementations;

public interface IVerification
{
    int id { get; set; }
    string email { get; set; }
    int code { get; set; }
    int resendCount { get; set; }
    int isValid { get; set; }
    string? type { get; set; }
    DateTime createdAt { get; set; }
    DateTime updatedAt { get; set; }
}