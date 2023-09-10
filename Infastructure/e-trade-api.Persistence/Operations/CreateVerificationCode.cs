using System.Text;

namespace e_trade_api.Persistence;

public static class CreateVerificationCode
{
    public static string CreateCode()
    {
        int length = 6;
        Random random = new();
        const string characters = "ABCDEFGHJKLMNPRSTUWYZ123456789";
        StringBuilder code = new(length);

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(characters.Length);
            code.Append(characters[index]);
        }
        return code.ToString();
    }
}
