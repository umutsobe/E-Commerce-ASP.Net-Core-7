namespace e_trade_api.Persistence;

public static class UrlBuilder
{
    public static string ProductUrlBuilder(string productName)
    {
        string trimName = productName.Trim();
        string guid = Guid.NewGuid().ToString().Substring(0, 12).Replace("-", "");

        string urlName = CharacterRegulatory(trimName);

        string url = urlName + "-" + guid;
        return url;
    }

    public static string CharacterRegulatory(string name) =>
        name.Replace("\"", "")
            .Replace("/", "")
            .Replace("!", "")
            .Replace("'", "")
            .Replace("^", "")
            .Replace("+", "")
            .Replace("*", "")
            .Replace("%", "")
            .Replace("&", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("=", "")
            .Replace("?", "")
            .Replace("_", "")
            .Replace(" ", "-")
            .Replace("@", "")
            .Replace("€", "")
            .Replace("¨", "")
            .Replace("~", "")
            .Replace(",", "")
            .Replace(";", "")
            .Replace(":", "")
            .Replace(".", "-")
            .Replace("Ö", "o")
            .Replace("ö", "o")
            .Replace("Ü", "u")
            .Replace("ü", "u")
            .Replace("ı", "i")
            .Replace("İ", "i")
            .Replace("ğ", "g")
            .Replace("Ğ", "g")
            .Replace("æ", "")
            .Replace("ß", "")
            .Replace("â", "a")
            .Replace("î", "i")
            .Replace("ş", "s")
            .Replace("Ş", "s")
            .Replace("Ç", "c")
            .Replace("ç", "c")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("{", "")
            .Replace("}", "")
            .Replace("#", "")
            .Replace("$", "")
            .Replace("£", "")
            .Replace("¥", "")
            .Replace("`", "")
            .Replace("´", "")
            .Replace("•", "")
            .Replace("✓", "")
            .Replace("™", "")
            .Replace("♦", "")
            .Replace("♣", "")
            .Replace("♠", "")
            .Replace("◆", "")
            .Replace("○", "")
            .Replace("♪", "")
            .Replace("♫", "")
            .Replace("♯", "")
            .Replace("★", "")
            .Replace("☆", "")
            .Replace("◊", "")
            .Replace("◘", "")
            .Replace("◙", "")
            .Replace("◦", "")
            .Replace("□", "")
            .Replace("■", "")
            .Replace("▪", "")
            .Replace("▫", "")
            .Replace("▲", "")
            .Replace("▼", "")
            .Replace("§", "")
            .Replace("¤", "")
            .Replace("▬", "")
            .Replace("¬", "")
            .Replace("¦", "")
            .Replace("◄", "")
            .Replace("►", "")
            .Replace("↨", "")
            .Replace("↑", "")
            .Replace("↓", "")
            .Replace("→", "")
            .Replace("←", "")
            .Replace("╔", "")
            .Replace("╗", "")
            .Replace("╚", "")
            .Replace("╝", "")
            .Replace("╩", "")
            .Replace("╦", "")
            .Replace("╠", "")
            .Replace("╣", "")
            .Replace("╬", "")
            .Replace("═", "")
            .Replace("║", "")
            .Replace("¸", "")
            .Replace("˛", "")
            .Replace("˘", "")
            .Replace("ˇ", "")
            .Replace("˙", "")
            .Replace("˝", "")
            .Replace("˚", "")
            .Replace("˜", "")
            .Replace("ˆ", "")
            .Replace("¯", "")
            .Replace("˛", "")
            .Replace("˝", "")
            .Replace("¸", "")
            .Replace("˚", "")
            .Replace("˜", "")
            .Replace("ˇ", "")
            .Replace("˘", "")
            .Replace("ˆ", "")
            .Replace("¯", "")
            .Replace("¨", "")
            .Replace("´", "")
            .Replace("˙", "")
            .Replace("\"", "");
}
