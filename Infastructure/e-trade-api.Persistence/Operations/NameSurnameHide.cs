namespace e_trade_api.Persistence;

public static class NameSurnameHide
{
    public static string Hide(string adSoyad)
    {
        if (string.IsNullOrWhiteSpace(adSoyad))
        {
            return adSoyad;
        }

        string[] adSoyadParcaları = adSoyad.Split(' ');

        if (adSoyadParcaları.Length < 2)
        {
            string _adinIlkHarfi = adSoyadParcaları[0].Substring(0, 1);
            string _gizlenmisAd = _adinIlkHarfi + new string('*', adSoyadParcaları[0].Length - 1);

            return _gizlenmisAd; //sadece ad gelmişse gizlenmiş adı döndürüyoruz
        }

        string adinIlkHarfi = adSoyadParcaları[0].Substring(0, 1);
        string soyadinIlkHarfi = adSoyadParcaları[1].Substring(0, 1);

        string gizlenmisAd = adinIlkHarfi + new string('*', adSoyadParcaları[0].Length - 1);
        string gizlenmisSoyad = soyadinIlkHarfi + new string('*', adSoyadParcaları[1].Length - 1);

        return gizlenmisAd + " " + gizlenmisSoyad;
    }
}
