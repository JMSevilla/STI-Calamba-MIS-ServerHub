namespace sti_sys_backend.Utilization;

public class GenerateVerificationCode
{
    private const int CodeLength = 6;
    private const string AllowedChars = "0123456789";

    public static string GenerateCode()
    {
        var random = new Random();
        string code = "";
        for (int i = 0; i < CodeLength; i++)
        {
            var index = random.Next(0, AllowedChars.Length);
            code += AllowedChars[index];
        }
        return code;
    }
}