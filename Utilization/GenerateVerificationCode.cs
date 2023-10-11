using System.Text;

namespace sti_sys_backend.Utilization;

public class GenerateVerificationCode
{
    private const int CodeLength = 6;
    private const string AllowedChars = "0123456789";
    private static Random random = new Random();

    public static string GenerateCode()
    {
        var code = new StringBuilder(CodeLength);
        
        for (int i = 0; i < CodeLength; i++)
        {
            int index = random.Next(0, AllowedChars.Length);
            code.Append(AllowedChars[index]);
        }

        return code.ToString();
    }
}