using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace e_trade_api.Infastructure;

public class ImageCompression
{
    public static Stream Compress(
        Stream srcImgStream,
        string fileFullName,
        int maxWidth,
        int maxHeight
    )
    {
        using var image = Image.FromStream(srcImgStream);

        float ratioX = (float)maxWidth / (float)image.Width;
        float ratioY = (float)maxHeight / (float)image.Height;
        float ratio = Math.Min(ratioX, ratioY);

        int newWidth = (int)(image.Width * ratio);
        int newHeight = (int)(image.Height * ratio);

        var bitmap = new Bitmap(image, newWidth, newHeight);

        var imgGraph = Graphics.FromImage(bitmap);
        imgGraph.SmoothingMode = SmoothingMode.Default;
        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

        imgGraph.DrawImage(image, 0, 0, newWidth, newHeight);

        var extension = Path.GetExtension(fileFullName).ToLower();
        ImageFormat imageFormat;
        ImageCodecInfo encoder;

        switch (extension)
        {
            case ".png":
            case ".gif":
                imageFormat = ImageFormat.Png;
                encoder = GetEncoder(imageFormat);
                break;
            case ".jpg":
            case ".jpeg":
                imageFormat = ImageFormat.Jpeg;
                encoder = GetEncoder(imageFormat);
                break;
            case ".webp":
                imageFormat = ImageFormat.Webp;
                encoder = GetEncoder(imageFormat);
                break;
            default:
                throw new ArgumentException("Unsupported image format.");
        }

        var resultStream = new MemoryStream();
        var encoderParameters = new EncoderParameters(1);
        var parameter = new EncoderParameter(Encoder.Quality, 90L);
        encoderParameters.Param[0] = parameter;

        bitmap.Save(resultStream, encoder, encoderParameters);

        bitmap.Dispose();
        imgGraph.Dispose();
        image.Dispose();

        return resultStream;
    }

    public static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }

        return null;
    }
}
