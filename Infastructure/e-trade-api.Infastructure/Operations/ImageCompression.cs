using SixLabors.ImageSharp.Formats.Webp;

namespace e_trade_api.Infastructure;

public static class CompressImage
{
    public static Stream Compress(
        Stream srcImgStream,
        string fullFileName,
        int quality,
        int maxWidth,
        int maxHeight
    )
    {
        var extension = Path.GetExtension(fullFileName).ToLower();

        srcImgStream.Position = 0;

        using var image = Image.Load(srcImgStream);

        float ratioX = (float)maxWidth / (float)image.Width;
        float ratioY = (float)maxHeight / (float)image.Height;
        float ratio = Math.Min(ratioX, ratioY);

        int newWidth = (int)(image.Width * ratio);
        int newHeight = (int)(image.Height * ratio);

        image.Mutate(x => x.Resize(newWidth, newHeight));

        var outputStream = new MemoryStream();

        if (extension == ".webp")
        {
            image.Save(outputStream, new WebpEncoder { Quality = quality });
        }
        else
        {
            // Convert to Webp
            fullFileName = Path.ChangeExtension(fullFileName, ".webp");
            image.Save(outputStream, new WebpEncoder { Quality = quality });
        }
        outputStream.Position = 0;

        return outputStream;
    }
}
