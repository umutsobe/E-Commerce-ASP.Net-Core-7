using System.Text;

namespace e_trade_api.Persistence;

public static class CreateVerificationCode
{
    private static readonly Random random = new();

    public static string Create()
    {
        int length = 6;
        const string characters = "ABCDEFGHIJKLMNOP0123456789";
        StringBuilder stringBuilder = new(length);

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(characters.Length);
            stringBuilder.Append(characters[index]);
        }

        return stringBuilder.ToString();
    }
}
